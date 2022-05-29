﻿using UnityEngine;
using System.Collections;

namespace Kareem.Fluid.SPH
{
    [RequireComponent(typeof(Fluid3D))]
    public class FluidRenderer3D : MonoBehaviour
    {
        public Fluid3D solver;
        public Material RenderParticleMat;
        public Color WaterColor;
        public bool IsRenderInShader = true;
        public bool IsBoundsDrawed = true;

        void OnRenderObject()
        {
            DrawParticle();
        }

        void DrawParticle()
        {
            float radius = 0.1f;
            RenderParticleMat.SetPass(0);
            RenderParticleMat.SetColor("_WaterColor", WaterColor);
            RenderParticleMat.SetBuffer("_ParticlesBuffer", solver.ParticlesBufferRead);
            // RenderParticleMat.SetFloat("_ParticleRadius", solver.BallRadius);
            FluidParticle3D[] particles = new FluidParticle3D[solver.NumParticles];
            solver.ParticlesBufferRead.GetData(particles);

            if (IsRenderInShader)
            {
                Graphics.DrawProceduralNow(MeshTopology.Points, solver.NumParticles);
            }
            else
            {
                DrawByExtension(particles, radius);
            }

            if (IsBoundsDrawed)
                DrawBounds();
        }

        void DrawByExtension(FluidParticle3D[] particles, float radius)
        {
            foreach (var item in particles)
            {
                // Debug.Log(item.Position);
                DebugExtension.DebugWireSphere(item.Position, WaterColor, radius, 0, false);
            }
        }

        void DrawBounds()
        {
            Vector3 offset = Vector3.zero;
            Vector3 range = solver.Range;

            Vector3 Start = new Vector3(offset.x, offset.y, offset.z);

            Vector3 EndX = new Vector3(range.x, 0, 0);
            Vector3 EndY = new Vector3(0, range.y, 0);
            Vector3 EndZ = new Vector3(0, 0, range.z);

            Vector3 EndXZ = EndX + EndZ;
            Vector3 EndXY = EndX + EndY;
            Vector3 EndYZ = EndZ + EndY;

            Vector3 EndXYZ = EndX + EndY + EndZ;

            Color colorBoundry = Color.blue;

            Debug.DrawLine(Start, Start + EndX, colorBoundry);
            Debug.DrawLine(Start, Start + EndY, colorBoundry);
            Debug.DrawLine(Start, Start + EndZ, colorBoundry);

            Debug.DrawLine(Start + EndXYZ, Start + EndXZ, colorBoundry);
            Debug.DrawLine(Start + EndXYZ, Start + EndXY, colorBoundry);
            Debug.DrawLine(Start + EndXYZ, Start + EndYZ, colorBoundry);

            Debug.DrawLine(Start + EndXY, Start + EndX, colorBoundry);
            Debug.DrawLine(Start + EndXY, Start + EndY, colorBoundry);

            Debug.DrawLine(Start + EndXZ, Start + EndX, colorBoundry);
            Debug.DrawLine(Start + EndXZ, Start + EndZ, colorBoundry);

            Debug.DrawLine(Start + EndYZ, Start + EndY, colorBoundry);
            Debug.DrawLine(Start + EndYZ, Start + EndZ, colorBoundry);
        }
    }
}