using UnityEngine;

namespace Sample
{
    /// <summary>
    /// 큐브 컬러를 흰색에서 빨간색으로 바꾸기
    /// 메터리얼 속성 값 접근하여 사용하기
    /// Render - material, sharedMaterial
    /// </summary>
    public class MaterialTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Renderer renderer;

        //Material의 속성값 관리하는 객체
        private MaterialPropertyBlock materialPropertyBlock;

        public Material redMaterial;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            renderer = GetComponent<Renderer>();

            //MaterialPropertyBlock 객체 생성
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            //키 입력 처리
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ChangeMaterial();
                //ChangeMaterialColor();
                ChangeSharedMaterialColor();
            }
        }
        #endregion

        #region Custom Method
        //메터리얼 바꿔치기
        private void ChangeMaterial()
        {
            renderer.material = redMaterial;
        }

        //메터리얼 컬러 변경하기
        private void ChangeMaterialColor()
        {
            renderer.material.SetColor("_BaseColor", Color.red);
            //renderer.sharedMaterial.SetColor("_BaseColor", Color.red);
        }

        //MaterialPropertyBlock 이용하여 sharedMaterial 컬러 변경하기
        private void ChangeSharedMaterialColor()
        {
            materialPropertyBlock.SetColor("_BaseColor", Color.red);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
        #endregion
    }
}