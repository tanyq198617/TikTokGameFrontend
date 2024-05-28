namespace UnityEngine.UI
{
    internal class EmptyRaycastInMain : MaskableGraphic
    {
        protected EmptyRaycastInMain()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}
