using System.Collections;
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

        public Button parent;

        private void Start()
        {
            parent = GetComponent<Button>();

            countText.gameObject.SetActive( false );
            parent.onClick.AddListener( () => OnButtonClick() );
        }

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
            if ( count == -1 )
                return;

            count++;
            countText.text = count == 0 ? string.Empty : count.ToString();
        }

        private void OnButtonClick()
        {
            overlay.gameObject.SetActive( false );

            if ( count == -1 )
            {
                mineImg.gameObject.SetActive( true );
                var gameController = FindObjectOfType<GameController>();
                gameController.OnGameover();
            }
            else
            {
                hasVisited = true;
                countText.gameObject.SetActive( true );
            }
        }

        private IEnumerator OpenBlock( float delay = 0.5f )
        {
            yield return new WaitForSeconds( delay );
        }
    }
}