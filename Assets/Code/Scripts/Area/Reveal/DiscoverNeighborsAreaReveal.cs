namespace SolitaireSettlement
{
    public class DiscoverNeighborsAreaReveal : IAreaReveal
    {
        public void OnAreaReveal(Area area)
        {
            var objs = new Area[AreaManager.Instance.AreaCountWidth, AreaManager.Instance.AreaCountHeight];
            for (var y = 0; y < AreaManager.Instance.AreaCountHeight; y++)
            {
                for (var x = 0; x < AreaManager.Instance.AreaCountHeight; x++)
                {
                    var i = y * AreaManager.Instance.AreaCountWidth + x;
                    objs[x, y] = AreaManager.Instance.GeneratedAreaComponents[i];
                }
            }

            var arrayX = area.Index % AreaManager.Instance.AreaCountWidth;
            var arrayY = area.Index / AreaManager.Instance.AreaCountWidth;

            if (arrayY + 1 < AreaManager.Instance.AreaCountHeight)
            {
                objs[arrayX, arrayY + 1].Discovered = true;
                objs[arrayX, arrayY + 1].gameObject.SetActive(true);
            }

            if (arrayY - 1 >= 0)
            {
                objs[arrayX, arrayY - 1].Discovered = true;
                objs[arrayX, arrayY - 1].gameObject.SetActive(true);
            }

            if (arrayX + 1 < AreaManager.Instance.AreaCountWidth)
            {
                objs[arrayX + 1, arrayY].Discovered = true;
                objs[arrayX + 1, arrayY].gameObject.SetActive(true);
            }

            if (arrayX - 1 >= 0)
            {
                objs[arrayX - 1, arrayY].Discovered = true;
                objs[arrayX - 1, arrayY].gameObject.SetActive(true);
            }
        }
    }
}