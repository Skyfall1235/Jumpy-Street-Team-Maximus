using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Grid))]
public class TileManager : MonoBehaviour
{
    const int width = 3;
    const int startingHeight = 2;
    int currentHeight = 0;

    public Grid Grid;

    public GameObject[] tilePrefabs;

    private Transform ManagerTransform
    {
        get { return transform; }
    }

    public UnityEvent OnTileGenerate = new();

    


    private void Start()
    {
        GenerateStarterTiles();
    }

    /// <summary>
    /// Generates a layer of tiles across the entire width at the starting height
    /// </summary>
    void GenerateStarterTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < startingHeight; y++)
            {
                currentHeight++;
                InstatiateTileAtCoorinate(x, y);
            }
        }
    }

    /// <summary>
    /// Generates a single horizontal row of tiles at the current height level
    /// </summary>
    void GenerateNextTile()
    {
        for (int x = 0; x < width; x++)
        {
            //generate the next set, and then bump the y val
            GameObject tile = InstatiateTileAtCoorinate(x, currentHeight);
            currentHeight++;
            tile.transform.parent = ManagerTransform;
        }
    }

    /// <summary>
    /// Spawns a tile at the given coordinates using the chosen tile prefab
    /// </summary>
    /// <param name="x">the x corrdinate of the grid</param>
    /// <param name="y"></param>
    /// <returns></returns>
    GameObject InstatiateTileAtCoorinate(int x, int y)
    {
        GameObject newTile = Instantiate(chooseTilePrefab(), Grid.GetCellCenterWorld(new Vector3Int((int)x, 0, (int)y)), Quaternion.identity);
        return newTile;
    }

    /// <summary>
    /// Selects a random tile prefab from the available set
    /// </summary>
    /// <returns></returns>
    GameObject chooseTilePrefab()
    {
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    int test(int y)
    {
        return y;
    }


    //TO DO: 
    /*  - randomly select a tile section to generate
     *  - asssign them to a centralized parent
     *  - add basic hazards
     *  - geneate tiles in advance (play around as needed)
     *  - call tile to start its own methods (such as coin placement or hazard interaction
     *  - 
     * 
     * 
     * 
     */
}
