using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Grid))]
public class TileManager : MonoBehaviour
{
    const int width = 5;

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
        //for each collum in the starting height
        for (int y = 0; y < startingHeight; y++)
        {
            GenerateTileRow();
            currentHeight++;
        }
    }

    GameObject GenerateTileRow()
    {
        //choose the tileset prefab list for the row
        GameObject[] ChosenTileSet = ChooseTilePrefabSet();
        GameObject randomRowFromSet = ChosenTileSet[Random.Range(0, ChosenTileSet.Length)];

        GameObject newTile = Instantiate(randomRowFromSet, Grid.GetCellCenterWorld(new Vector3Int(0, 0, currentHeight)), Quaternion.identity);
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
