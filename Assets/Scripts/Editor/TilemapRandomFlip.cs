using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Herramienta de Editor para que los tilemaps se vean menos repetidos.
/// Selecciona un Tilemap (o el Grid que lo contiene) y usa el menú Tools > Tilemap.
/// Solo voltea en X, que es la única transformación que mantiene la forma de un tile isométrico.
/// Todo se puede deshacer con Ctrl+Z.
/// </summary>
public static class TilemapRandomFlip
{
    private const string MenuRoot = "Tools/Tilemap/";

    private static readonly Matrix4x4 FlipX = Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));

    [MenuItem(MenuRoot + "Voltear tiles al azar (Flip X)")]
    private static void RandomizeFlip()
    {
        int flipped = 0;
        int total = 0;

        foreach (Tilemap tilemap in GetSelectedTilemaps())
        {
            Undo.RegisterCompleteObjectUndo(tilemap, "Voltear tiles al azar");

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos))
                    continue;

                bool flip = Random.value < 0.5f;

                // Quitar el bloqueo de transformación de esta celda para poder voltearla
                tilemap.SetTileFlags(pos, TileFlags.None);
                tilemap.SetTransformMatrix(pos, flip ? FlipX : Matrix4x4.identity);

                total++;
                if (flip)
                    flipped++;
            }

            MarkDirty(tilemap);
        }

        Debug.Log($"[TilemapRandomFlip] {flipped} de {total} tiles volteados.");
    }

    [MenuItem(MenuRoot + "Quitar volteos")]
    private static void ResetFlip()
    {
        int total = 0;

        foreach (Tilemap tilemap in GetSelectedTilemaps())
        {
            Undo.RegisterCompleteObjectUndo(tilemap, "Quitar volteos");

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos))
                    continue;

                tilemap.SetTileFlags(pos, TileFlags.None);
                tilemap.SetTransformMatrix(pos, Matrix4x4.identity);
                total++;
            }

            MarkDirty(tilemap);
        }

        Debug.Log($"[TilemapRandomFlip] Volteo quitado en {total} tiles.");
    }

    // Los menús solo se activan si hay algún Tilemap seleccionado
    [MenuItem(MenuRoot + "Voltear tiles al azar (Flip X)", true)]
    [MenuItem(MenuRoot + "Quitar volteos", true)]
    private static bool HasTilemapSelected()
    {
        return GetSelectedTilemaps().Count > 0;
    }

    private static List<Tilemap> GetSelectedTilemaps()
    {
        var result = new List<Tilemap>();
        foreach (GameObject go in Selection.gameObjects)
        {
            foreach (Tilemap tilemap in go.GetComponentsInChildren<Tilemap>())
            {
                if (!result.Contains(tilemap))
                    result.Add(tilemap);
            }
        }
        return result;
    }

    private static void MarkDirty(Tilemap tilemap)
    {
        EditorUtility.SetDirty(tilemap);
        if (tilemap.gameObject.scene.IsValid())
            EditorSceneManager.MarkSceneDirty(tilemap.gameObject.scene);
    }
}
