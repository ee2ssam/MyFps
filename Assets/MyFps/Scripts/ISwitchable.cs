using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 온/오프(활성화/비활성화) 기능 정의
    /// </summary>
    public interface ISwitchable
    {
        public bool IsActive { get; set; }

        public void Activate();
        public void Deactivate();
    }
}