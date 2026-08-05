using UnityEngine;
using UnityEngine.InputSystem;

namespace MySample
{
    /// <summary>
    /// 큐브 컬러를 흰색에서 빨간색으로 바꾸기
    /// 메터리얼 바꿔치기로 컬러 바꾸기
    /// 직접 메터리얼의 컬러를 빨간색으로 바꾸기
    /// </summary>
    public class MaterialTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Renderer renderer;

        //인풋
        public InputActionReference jumpAction;

        public Material damagedMaterial;
        private Material originMaterial;

        //Material의 속성값을 관리하는 객체
        private MaterialPropertyBlock materialPropertyBlock;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            renderer = GetComponent<Renderer>();

            //MaterialPropertyBlock 객체 생성
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            //초기화
            //originMaterial = renderer.material;
        }

        private void OnEnable()
        {
            jumpAction.action.Enable();
        }

        private void OnDisable()
        {
            jumpAction.action.Disable();
        }

        private void Update()
        {
            //스페이스바를 누르면 큐브 컬러 변경
            if(jumpAction.action.WasPressedThisFrame())
            {
                //Debug.Log("큐브 컬러 변경");
                //ChangeMaterial();
                //ChangeMaterialColor();
                ChangeSharedMaterialColor();
            }
        }
        #endregion

        #region Custom Method
        // 메터리얼 바꿔치기
        private void ChangeMaterial()
        {
            renderer.material = damagedMaterial;
        }

        private void ResetMaterial()
        {
            renderer.material = originMaterial;
        }

        //직접 메터리얼의 컬러를 빨간색으로 바꾸기
        private void ChangeMaterialColor()
        {
            renderer.material.SetColor("_BaseColor", Color.red);
            //renderer.sharedMaterial.SetColor("_BaseColor", Color.red);
        }

        //해당 오브젝트만 컬러 변경, 배칭도 깨지지 않고
        //MaterialPropertyBlock를 이용하여 sharedMaterial의 컬러를 변경하기
        private void ChangeSharedMaterialColor()
        {
            materialPropertyBlock.SetColor("_BaseColor", Color.red);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
        #endregion
    }
}