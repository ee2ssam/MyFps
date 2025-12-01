using UnityEngine;

namespace MyFps
{
    public class PickupKey : PickupItem
    {
        protected override void DoAction()
        {
            //Debug.Log("키를 획득하였습니다");
            PlayerStats.Instance.GainPuzzleItem(PuzzleItem.Key01);

            //아이템 킬
            Destroy(gameObject);
        }
    }
}