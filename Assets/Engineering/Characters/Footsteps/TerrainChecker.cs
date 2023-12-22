using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChecker
{
    public float[] GetTextureMix(Vector3 playerPos, Terrain t) {
        Vector3 tPos = t.transform.position;
        TerrainData tData = t.terrainData;

        int mapX = (int)(((playerPos.x - tPos.x) / tData.size.x) * tData.alphamapWidth);
        int mapZ = (int)(((playerPos.z - tPos.z) / tData.size.z) * tData.alphamapHeight);

        float[,,] splatmapData = tData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

        for (int i = 0; i < cellMix.Length; i++) {
            cellMix[i] = splatmapData[0, 0, i];
        }
        return cellMix;
    }

    public string GetLayerName(Vector3 playerPos, Terrain t) {
        float[] mix = GetTextureMix(playerPos, t);
        float maxMix = 0;
        int maxIndex = 0;

        for (int i = 0; i < mix.Length; i++) {
            if (mix[i] > maxMix) {
                maxIndex = i;
                maxMix = mix[i];
            }
        }

        return t.terrainData.terrainLayers[maxIndex].name;
    }   

    public int GetLayerId(Vector3 playerPos, Terrain t) {
        float[] mix = GetTextureMix(playerPos, t);
        float maxMix = 0;
        int maxIndex = 0;

        for (int i = 0; i < mix.Length; i++) {
            if (mix[i] > maxMix) {
                maxIndex = i;
                maxMix = mix[i];
            }
        }

        return t.terrainData.terrainLayers[maxIndex].GetInstanceID();
    }
}
