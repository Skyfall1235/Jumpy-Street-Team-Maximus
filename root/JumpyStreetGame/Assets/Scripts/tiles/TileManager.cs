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
            //InstatiateTileAtCoorinate(y);
            
        }
    }

    GameObject GenerateTileRow()
    {
        GameObject tileRow = new GameObject();
        tileRow.transform.parent = ManagerTransform;
        for (int x = 0; x < width; x++)
        {
            //choose the tileset prefab list
            GameObject[] ChosenTileSet = ChooseTilePrefabSet();

            if (ChosenTileSet == baseTilePrefabs)
            {
                //generate the next set, and then bump the y val
                GameObject tile = InstatiateTileAtCoorinate(x, currentHeight + 1, GiveRandomBaseTileSet());

                //set the transform, and move on
                tile.transform.parent = tileRow.transform;
            }
            else
            {
                //no need to figure out the type, they work the same.
                //spawn first prefab first, fill with width-2, and then slap on the end tile.

            }
            currentHeight++;
        }
        return tileRow;
    }



    /// <summary>
    /// Spawns a tile at the given coordinates using the chosen tile prefab
    /// </summary>
    /// <param name="x">the x corrdinate of the grid</param>
    /// <param name="y">the y coordinate of the grid</param>
    /// <param name="tileObject">The prefab object we want to generate</param>
    /// <returns>the tile that was generated based on TileObject</returns>
    GameObject InstatiateTileAtCoorinate(int x, int y, GameObject tileObject)
    {
        GameObject newTile = Instantiate(tileObject, Grid.GetCellCenterWorld(new Vector3Int((int)x, 0, (int)y)), Quaternion.identity);
        newTile.transform.parent = ManagerTransform;
        return newTile;
    }


    GameObject[] ChooseTilePrefabSet()
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
    

    GameObject GiveRandomBaseTileSet()
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
