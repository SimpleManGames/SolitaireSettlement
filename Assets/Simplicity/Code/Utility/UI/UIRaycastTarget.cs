using UnityEngine.UI;

namespace Simplicity
{
    public class UIRaycastTarget : Graphic
    {
        public override void SetMaterialDirty()
        {
            return;
        }

        public override void SetVerticesDirty()
        {
            return;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}