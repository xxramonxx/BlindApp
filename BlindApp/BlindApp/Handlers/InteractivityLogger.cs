﻿using BlindApp.Database.Tables;
using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MathNet.Numerics;
using System.Numerics;

namespace BlindApp
{
    public static class InteractivityLogger
    {

        public static async void Bu()
        {
            //var Folder = await FileSystem.Current.LocalStorage.CreateFolderAsync("myFolder",
            //                CreationCollisionOption.OpenIfExists);
            //IFile file = await Folder.CreateFileAsync("test.txt", 
            //                  CreationCollisionOption.ReplaceExisting);
            //await file.WriteAllTextAsync("42");

            //IFolder rootFolder = FileSystem.Current.LocalStorage;

            //IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder",
            //    CreationCollisionOption.OpenIfExists);
            
            //IFile file = await folder.CreateFileAsync("answer.txt",
            //    CreationCollisionOption.ReplaceExisting);
            //await file.
        }

        public static async void Test()
        {

            //var P1 = new Vector3(
	           // (float)290,
	           // (float)-290,
	           // 0f);
            //var P2 = new Vector3(
            //    (float)290,
            //    (float)-650,
            //    0f);
            //var P3 = new Vector3(
            //    (float)720,
            //    (float)-300,
            //    0f);
            
            //var DistA = 450;
            //var DistB = 390;
            //var DistC = 450;

            //var ex = (P2 - P1) / Norm(P2 - P1);
            //var i = Vector3.Dot(ex, P3 - P1);
            //var ey = (P3 - P1 - i * ex) / (Norm(P3 - P1 - i * ex));
            //var d = Norm(P2 - P1);
            //var j = Vector3.Dot(ey, (P3 - P1));

            //float x = (float) (Math.Pow(DistA, 2) - Math.Pow(DistB, 2) + Math.Pow(d, 2)) / (2 * d);
            //float y = (float) ((Math.Pow(DistA, 2) - Math.Pow(DistC, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j)) - ((i / j) * x);
            //var result = P1 + (x * ex) + (y * ey);

            //var resultPoint = new Point(result.X, result.Y);
            //Debug.WriteLine("jupi");


        }

        private static float Norm(Vector3 v)
        {
            return (float) Math.Sqrt(
                Math.Pow(v.X, 2) +
                Math.Pow(v.Y, 2) +
                Math.Pow(v.Z, 2));
        }
    }
}