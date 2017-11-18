using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Minesweeper
{
    public class Point
    {
        public int i;
        public int j;

        public Point( int i, int j )
        {
            this.i = i;
            this.j = j;
        }

        public override string ToString()
        {
            return "(i " + i + ", j " + j + ")";
        }
    }

    public class BlockObject : MonoBehaviour
    {
        #region globals
        public int count;
        public bool hasVisited;

        public Image overlay;
        public Image mineImg;
        public Text countText;

        public Point point;

        private GameController gameController;
        #endregion

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();

            countText.gameObject.SetActive( false );
            var button = GetComponent<Button>();
            button.onClick.AddListener( () => OnButtonClick() );
        }

        public void Setup( int count, Point point, bool hasVisited = false )
        {
#if USE_CHEAT
            if ( count == -1 )
                overlay.color = Color.green;
#endif
            this.count = count;
            this.hasVisited = hasVisited;
            this.point = point;

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
                gameController.OnGameover();
            }
            else
            {
                hasVisited = true;
                countText.gameObject.SetActive( true );

                StartCoroutine( OpenBlock() );
            }
        }

        private void DisableThisBlock()
        {
            hasVisited = true;
            gameController.OnBlockVisited();
            overlay.gameObject.SetActive( false );

            if ( count != -1 )
                countText.gameObject.SetActive( true );
            else
                mineImg.gameObject.SetActive( true );
        }

        private void DisableAdjacentBlocks()
        {
            var blocks = gameController.GetObjectsByIndex( this );
            for ( var i = 0; i < blocks.Length; i++ )
            {
                if ( blocks[i] == null )
                    break;

                StartCoroutine( blocks[i].OpenBlock() );
            }
        }

        private IEnumerator OpenBlock( float delay = 0.25f )
        {
            if ( hasVisited == true || count == -1 )
                yield return null;

            DisableThisBlock();

            yield return new WaitForSeconds( delay );


            DisableAdjacentBlocks();
        }
    }
}