using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Grid))]
public class TileManager : MonoBehaviour
{
    const int width = 10;
    [Range(0, 100)]
    [SerializeField] private int startingHeight = 2;
    [SerializeField] private int currentHeight;

    public Grid Grid;

    public GameObject[] baseTilePrefabs;
    public GameObject[] roadTilePrefabs;
    public GameObject[] riverTilePrefabs;

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
        
        for (int y = 0; y < startingHeight; y++)
        {
            currentHeight++;
            InstatiateTileAtCoorinate(y);
            
        }
    }

    /// <summary>
    /// Generates a single horizontal row of tiles at the current height level
    /// </summary>
    void GenerateNextTileSet()
    {
        for (int x = 0; x < width; x++)
        {
            //generate the next set, and then bump the y val
            GameObject tile = InstatiateTileAtCoorinate(x, currentHeight+1);
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
        GameObject newTile = Instantiate(chooseTilePrefabSet(), Grid.GetCellCenterWorld(new Vector3Int((int)x, 0, (int)y)), Quaternion.identity);
        newTile.transform.parent = ManagerTransform;
        return newTile;
    }


    GameObject[] chooseTilePrefabSet()
    {
        int tileSetChoice = Random.Range(1, 11);//its exclusive :(
        //choose if its a road, river, or normal tile
        //give bias to the normal tile
        switch (tileSetChoice)
        {
            case 1: case 2:
                return roadTilePrefabs;
            case 3:case 4:
                return riverTilePrefabs;
            default:
                return baseTilePrefabs;
        }
    }
    GameObject GenerateTileRow(int height)
    {
        GameObject
    }

    GameObject GiveRandomTile()
    {
        return baseTilePrefabs[Random.Range(0, baseTilePrefabs.Length)];
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
