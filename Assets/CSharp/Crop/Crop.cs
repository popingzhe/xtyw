using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;

    private int harvestActionCount;

    private TileDetails tileDetails;

    private Animator animator;

    public Transform playerTransform;
    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    public void ProcessToolAction(ItemDetails tool,TileDetails tile)
    {
        tileDetails = tile;
        //���ߴ���
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;
        
       animator = GetComponentInChildren<Animator>();
        //���������
        if (harvestActionCount < requireActionCount )
        {
            harvestActionCount++;
            //���������� 
            if (animator != null && cropDetails.hasAimation)
            {
                if(playerTransform.position.x < transform.position.x)
                {
                    animator.SetTrigger("treeRotationRight");
                }
                else
                {
                    animator.SetTrigger("treeRotationLeft");
                }
            }

            //��������
            //��������
        }

        if( harvestActionCount >= requireActionCount )
        {
            if (cropDetails.generateAtPlayerPosition || !cropDetails.hasAimation)
            {
                //����ũ����
                SpawnHarvestItems();
            }else if(cropDetails.hasAimation)
            {
                if (playerTransform.position.x < transform.position.x)
                {
                    animator.SetTrigger("faling Right");
                }
                else
                {
                    animator.SetTrigger("faling left");
                }
                StartCoroutine(HarvestAfterAnimation());
            }
        }
    }

    private IEnumerator HarvestAfterAnimation()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            yield return null;
        }

        SpawnHarvestItems();
        //ת����Ʒ
        if (cropDetails.transferItemID > 0)
        {
            CreateTransferCrop();
        }
    }

    private void CreateTransferCrop()
    {
        tileDetails.seedItem = cropDetails.transferItemID;
        tileDetails.daySinceLastHarvest = -1;
        tileDetails.growthDays = 0;
        EventHander.CallRefreshCurrentMap();
    }

    public void SpawnHarvestItems()
    {
        for(int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;

            if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
            {
                amountToProduce = cropDetails.producedMinAmount[i];
            }
            else//�������
            {
                amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
            }

            //���ɹ�ʵ
            for(int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPosition)
                {
                    EventHander.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                }
                else//����������Ʒ
                {
                    var dirX = transform.position.x > playerTransform.position.x ? 1 : -1;
                    var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spwanRadius.x * dirX),
                        transform.position.y + Random.Range(-cropDetails.spwanRadius.y, cropDetails.spwanRadius.y), 0);
                    EventHander.CallInstantiateiyemInSence(cropDetails.producedItemID[i], spawnPos);
              }      
            }

        }

    if(tileDetails != null)
        {
            tileDetails.daySinceLastHarvest++;

            //�Ƿ��������
            if(cropDetails.daysToRegrow >  0 && tileDetails.daySinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //ˢ������
                EventHander.CallRefreshCurrentMap();
            }
            else //��������
            {
                tileDetails.daySinceLastHarvest = -1;
                tileDetails.seedItem = -1;

            }

            Destroy(gameObject);
        }
    
    }



}
