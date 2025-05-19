using UnityEngine;
using TMPro;

namespace MyFps
{
    //������ ���ͷ�Ƽ�� �׼� ����
    public class DoorCellOpen : MonoBehaviour
    {
        #region Variables
        //���� �÷��̾���� �Ÿ�
        private float theDistance;

        //�׼� UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        //ũ�ν����
        public GameObject extraCross;

        [SerializeField]
        private string action = "Open The Door";

        //�ִϸ��̼�
        public Animator animator;

        //�ִ� �Ķ���� ��Ʈ��
        private string  paramIsOpen = "IsOpen";
        #endregion

        #region Unity Event Method

        private void Update()
        {
            //���� �÷��̾���� �Ÿ� ��������
            theDistance = PlayerCasting.distanceFromTarget;
        }

        private void OnMouseOver()
        {
            extraCross.SetActive(true);

            if (theDistance <= 2f)
            {
                ShowActionUI();

                //TODO : New Input System ��ü ����
                //Ű�Է� üũ
                if(Input.GetKeyDown(KeyCode.E))
                {
                    //UI �����, ������, �浹ü ����
                    HideActionUI();
                    animator.SetBool(paramIsOpen, true);        //�� ���� �ִϸ��̼� ����
                    this.GetComponent<BoxCollider>().enabled = false; //�� �浹ü ����
                }
            }
            else
            {
                HideActionUI();
            }
        }

        private void OnMouseExit()
        {
            extraCross.SetActive(false);

            HideActionUI();
        }
        #endregion

        #region Custom Method
        //Action UI �����ֱ�
        private void ShowActionUI()
        {
            actionUI.SetActive(true);
            actionText.text = action;
        }

        //Action UI �����
        private void HideActionUI()
        {
            actionUI.SetActive(false);
            actionText.text = "";
        }
        #endregion
    }
}
