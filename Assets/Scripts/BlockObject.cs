using UnityEngine;
using UnityEngine.UI;

namespace Game.Minesweeper
{
    public class BlockObject : MonoBehaviour
    {
        public int count;
        public bool hasVisited;

        public Image overlay;
        public Image mineImg;
        public Text countText;

        public void Setup( int count, bool hasVisited = false )
        {
            this.count = count;
            this.hasVisited = hasVisited;

            SetCountText();
        }

        public void SetCountText()
        {
            countText.text = count == 0 ? string.Empty : count.ToString();
        }

        public void SetCounter()
        {
            count++;
            countText.text = count == 0 ? string.Empty : count.ToString();
        }
    }
}