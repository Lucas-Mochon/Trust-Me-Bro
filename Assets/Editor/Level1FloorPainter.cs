using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Level1FloorPainter
{
    const string TILEMAP_NAME = "Tilemap";
    const string TILE_PATH_BASE = "Assets/Tilemaps/TilePalette/";

    [MenuItem("Tools/Trust Me Bro/Draw Level1 Floor")]
    static void DrawLevel1Floor()
    {
        Tilemap tilemap = FindTilemap();
        if (tilemap == null) return;

        TileBase surfaceTile = LoadTile("TileSet_1.asset");
        TileBase subTile     = LoadTile("TileSet_2.asset");
        if (surfaceTile == null || subTile == null) return;

        Undo.RecordObject(tilemap, "Draw Level1 Floor");
        tilemap.ClearAllTiles();

        // ── Sol principal ──────────────────────────────────────────
        // Surface (rangée du dessus) — sur laquelle le joueur marche
        PaintRow(tilemap, surfaceTile, xFrom: -3, xTo: 25, y: -1);
        // Sous-sol (rangée du dessous) — épaisseur visuelle
        PaintRow(tilemap, subTile,     xFrom: -3, xTo: 25, y: -2);

        // ── Mur gauche (bord de la carte) ─────────────────────────
        PaintColumn(tilemap, subTile, x: -4, yFrom: -3, yTo:  4);
        PaintColumn(tilemap, subTile, x: -3, yFrom: -3, yTo: -3);

        // ── Plateforme suspendue centrale (pure déco Level 1) ─────
        // Visible mais accessible par saut depuis le sol
        TileBase platTile = LoadTile("TileSet_3.asset") ?? surfaceTile;
        PaintRow(tilemap, platTile, xFrom: 5, xTo: 8, y: 2);

        // ── Sol sous la plateforme (ferme l'espace) ───────────────
        PaintRow(tilemap, subTile, xFrom: 5, xTo: 8, y: 1);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[Trust Me Bro] Level1 floor drawn — " +
                  $"{tilemap.GetUsedTilesCount()} tuiles posées.");
    }

    // ─────────────────────────────────────────────────────────────

    static Tilemap FindTilemap()
    {
        var go = GameObject.Find(TILEMAP_NAME);
        if (go == null)
        {
            Debug.LogError($"[Level1FloorPainter] GameObject '{TILEMAP_NAME}' " +
                           "introuvable dans la scène active.");
            return null;
        }
        var tm = go.GetComponent<Tilemap>();
        if (tm == null)
            Debug.LogError($"[Level1FloorPainter] '{TILEMAP_NAME}' n'a pas de Tilemap.");
        return tm;
    }

    static TileBase LoadTile(string assetFile)
    {
        string path = TILE_PATH_BASE + assetFile;
        var tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
        if (tile == null)
            Debug.LogError($"[Level1FloorPainter] Tile introuvable : {path}");
        return tile;
    }

    static void PaintRow(Tilemap tm, TileBase tile, int xFrom, int xTo, int y)
    {
        for (int x = xFrom; x <= xTo; x++)
            tm.SetTile(new Vector3Int(x, y, 0), tile);
    }

    static void PaintColumn(Tilemap tm, TileBase tile, int x, int yFrom, int yTo)
    {
        for (int y = yFrom; y <= yTo; y++)
            tm.SetTile(new Vector3Int(x, y, 0), tile);
    }
}
