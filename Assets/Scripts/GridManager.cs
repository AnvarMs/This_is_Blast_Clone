using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public int rows = 10;
    public int cols = 10;
    private int totalBlock;
    public float fallSpeed = 2f;
    [Range(0,5)]
    public int colorPattern; // Controls the pattern logic from the Inspector
    List<Color> colors = new List<Color>();
    GameObject[,] gridPrefab;
    private static GridManager instance;
    public static GridManager Instance {  get { return instance; } }
    public UnityEvent onCubeDestroy;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Initialize color palette
        GenerateColors();
        GenerateGrid();
        onCubeDestroy.AddListener(CheckAllBlockAreDestroyed);
        totalBlock = rows * cols;
    }

    void GenerateColors()
    {
        // Define a color palette
        colors.Add(Color.red);
        colors.Add(Color.blue);
        colors.Add(Color.green);
        colors.Add(Color.yellow);
        colors.Add(Color.cyan);
        colors.Add(Color.magenta);
    }

    void GenerateGrid()
    {
        gridPrefab = new GameObject[cols, rows];

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(x, 0, y); // Use consistent axis setup
                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, transform);
                gridPrefab[x, y] = block;

                // Assign colors or other initialization as needed
                block.GetComponent<Block>().Initialize(GetColorBasedOnPattern(x, y));
            }
        }
    }


    Color GetColorBasedOnPattern(int x, int y)
    {
        // Number of rows in each color block for the new pattern
        int rowsPerColor = 3;

        // Use `colorPattern` to control which logic to apply
        switch (colorPattern)
        {
            case 0: // Horizontal stripes
                return colors[y % colors.Count];

            case 1: // Vertical stripes
                return colors[x % colors.Count];

            case 2: // Checkerboard pattern
                return colors[(x + y) % colors.Count];

            case 3: // Diagonal gradient
                return colors[(x + y) % colors.Count];

            case 4: // Random color
                return colors[Random.Range(0, colors.Count)];

            case 5: // Row blocks (new pattern)
                return colors[(y / rowsPerColor) % colors.Count];

            default: // Default pattern (e.g., solid color)
                return colors[0];
        }
    }

    public IEnumerator ApplyGravity(int col)
    {
        bool isLastRow = false;
        for (int y = 1; y < rows; y++)
        {

            if (gridPrefab[col, y] != null)
            {
                isLastRow = true;
                gridPrefab[col, y].transform
                   .DOMoveZ(y-1, fallSpeed)
                   .SetEase(Ease.InOutSine);

                // Update grid
                gridPrefab[col, y - 1] = gridPrefab[col, y];
                gridPrefab[col, y] = null;
            }
           


        }
        if(!isLastRow) gridPrefab[col, 0] = null;

        yield return new WaitForSeconds(fallSpeed);
    }


    private void CheckAllBlockAreDestroyed()
    {
        totalBlock--;
        if(totalBlock == 0)
        {
            GameManager.Instance.GameWin();
        }

    }
    public void CheckGameOver()
    {
        // Check if all slots are equipped (all true)
        bool allSlotsEquipped = true;

        foreach (bool isFill in FightSlotManager.Instance.slotFilld)
        {
            Debug.Log("The Slot"+isFill);
            if (!isFill)
            {
                allSlotsEquipped = false;
                break; // Stop checking further if any slot is not equipped
            }
        }
        bool IsShootersWaiting = false;

        foreach(Shooter shooter in FightBlockHolder.Instance.shooterList)
        {
            if (shooter == null) continue;
            if(shooter.isOnWar && shooter.isWaitingtoFire)
            {
                IsShootersWaiting=true;
            }
        }

        Debug.Log(IsShootersWaiting);
        // Game over if blocks are remaining and all slots are equipped
        if (allSlotsEquipped  && IsShootersWaiting && totalBlock > 0)
        {
           
            Debug.Log(totalBlock > 0);
            GameManager.Instance.GameOver();
        }
    }




    public GameObject GetBlock(int row, int col)
    {
        return gridPrefab[col, row];
    }
}
