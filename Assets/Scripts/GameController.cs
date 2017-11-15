using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Minesweeper
{
    public class GameController : MonoBehaviour
    {
        public BlockObject blockInfoPrefab;
        public BlockObject[,] blockObjects;
        public Transform parent;

        public Text gameOverText;

        public int gridSize = 3;
        public int mineCount = 3;

        void Start()
        {
            //gridSize = 4;  //PlayerPrefs.GetInt( "GridSize" );
            //mineCount = 4; //PlayerPrefs.GetInt( "MineCount" );

            blockObjects = new BlockObject[gridSize, gridSize];

            SetupGrid();
            SetupAdjacentCounter();
        }

        private int[] SetupMineIndex()
        {
            var array = new int[mineCount];
            var size = gridSize * gridSize;

            for ( var i = 0; i < mineCount; i++ )
            {
                array[i] = UnityEngine.Random.Range( 0, size );
            }

            return array;
        }

        private bool Contains( int[] array, int value )
        {
            for ( var i = 0; i < array.Length; i++ )
            {
                if ( array[i] == value )
                    return true;
            }

            return false;
        }

        public void OnGameover()
        {
            gameOverText.gameObject.SetActive( true );

            for ( var j = 0; j < gridSize; j++ )
            {
                for ( var i = 0; i < gridSize; i++ )
                {
                    if ( blockObjects[i, j].count == -1 )
                    {
                        blockObjects[i, j].overlay.gameObject.SetActive( false );
                        blockObjects[i, j].mineImg.gameObject.SetActive( true );
                    }
                }
            }

            StartCoroutine( ShowGameoverScreen() );
        }

        public IEnumerator ShowGameoverScreen( float delay = 4f )
        {
            yield return new WaitForSeconds( delay );

            UnityEngine.SceneManagement.SceneManager.LoadScene( "Gameover" );
        }

        private bool IsMine( int index )
        {
            var mineIndex = 0;
            var selectedArray = SetupMineIndex();

            if ( mineIndex < mineCount )
            {
                if ( Contains( selectedArray, index ) )
                {
                    mineIndex++;
                    return true;
                }
            }

            return false;
        }

        private void SetupGrid()
        {
            var buttonRect = blockInfoPrefab.GetComponent<Button>().GetComponent<RectTransform>();
            var index = 0;

            for ( var j = 0; j < gridSize; j++ )
            {
                for ( var i = 0; i < gridSize; i++ )
                {
                    //instantite
                    var block = Instantiate( blockInfoPrefab, Vector2.zero, Quaternion.identity, parent ) as BlockObject;

                    //set name
                    block.gameObject.name = "Block_" + index;

                    //set position
                    block.GetComponent<RectTransform>().anchoredPosition = new Vector2( i * buttonRect.rect.width, -j * buttonRect.rect.height );
                    block.Setup( IsMine( index++ ) ? -1 : block.count, new Point( i, j ) );

                    //save to array
                    blockObjects[i, j] = block;
                }
            }
        }

        private void SetupAdjacentCounter()
        {
            for ( var j = 0; j < gridSize; j++ )
            {
                for ( var i = 0; i < gridSize; i++ )
                {
                    if ( blockObjects[i, j].count == -1 )
                        PopulateAdjacentBlockCounter( i, j );
                }
            }
        }

        private void PopulateAdjacentBlockCounter( int i, int j )
        {
            //left
            if ( i > 0 )
            {
                blockObjects[i - 1, j].SetCounter();
            }

            //right
            if ( i < gridSize - 1 )
            {
                blockObjects[i + 1, j].SetCounter();
            }

            //up
            if ( j > 0 )
            {
                blockObjects[i, j - 1].SetCounter();
            }

            //down
            if ( j < gridSize - 1 )
            {
                blockObjects[i, j + 1].SetCounter();
            }

            //top left
            if ( i > 0 && j > 0 )
            {
                blockObjects[i - 1, j - 1].SetCounter();
            }

            //top right
            if ( i < gridSize - 1 && j > 0 )
            {
                blockObjects[i + 1, j - 1].SetCounter();
            }

            //bottom left
            if ( i > 0 && j < gridSize - 1 )
            {
                blockObjects[i - 1, j + 1].SetCounter();
            }

            //bottom right
            if ( i < gridSize - 1 && j < gridSize - 1 )
            {
                blockObjects[i + 1, j + 1].SetCounter();
            }
        }

        public BlockObject[] GetObjectsByIndex( BlockObject caller )
        {
            var index = 0;
            var blocks = new BlockObject[8];
            var i = caller.point.i;
            var j = caller.point.j;

            //left
            if ( i > 0 )
            {
                var block = blockObjects[i - 1, j];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //right
            if ( i < gridSize - 1 )
            {
                var block = blockObjects[i + 1, j];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //up
            if ( j > 0 )
            {
                var block = blockObjects[i, j - 1];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //down
            if ( j < gridSize - 1 )
            {
                var block = blockObjects[i, j + 1];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //top left
            if ( i > 0 && j > 0 )
            {
                var block = blockObjects[i - 1, j - 1];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //top right
            if ( i < gridSize - 1 && j > 0 )
            {
                var block = blockObjects[i + 1, j - 1];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //bottom left
            if ( i > 0 && j < gridSize - 1 )
            {
                var block = blockObjects[i - 1, j + 1];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            //bottom right
            if ( i < gridSize - 1 && j < gridSize - 1 )
            {
                var block = blockObjects[i + 1, j + 1];
                if ( !block.hasVisited && block.count != -1 )
                {
                    blocks[index++] = block;
                }
                else
                    return blocks;
            }

            return blocks;
        }
    }
}