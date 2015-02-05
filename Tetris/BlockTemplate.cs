using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class BlockTemplate
    {
        public Points[] GetBlock(string block)
        {
            switch (block)
            {
                case "T": // starting point of T
                    return new Points[]{
                        new Points(0,0),
                        new Points(1,0),
                        new Points(0,-1),                        
                        new Points(0,1)};

                case "Line": // starting point of Line
                    return new Points[]{
                        new Points(0,0),
                        new Points(0,-1),
                        new Points(0,1),
                        new Points(0,2)};

                case "Square": // starting point of Box
                    return new Points[]{
                        new Points(0,0),
                        new Points(0,1),
                        new Points(1,0),
                        new Points(1,1)};

                case "RightZ": // starting point of Right Z
                    return new Points[]{
                        new Points(0,0),
                        new Points(0,1),
                        new Points(1,-1),
                        new Points(1,0)};

                case "LeftZ": // starting point of Left Z
                    return new Points[]{
                        new Points(0,0),
                        new Points(0,-1),
                        new Points(1,0),
                        new Points(1,1)};

                case "RightL": // starting point of Right L
                    return new Points[]{
                        new Points(0,0),
                        new Points(1,0),
                        new Points(-1,0),
                        new Points(-1,1)};

                case "LeftL": // starting point of Left L                    
                    return new Points[]{
                        new Points(0,0),
                        new Points(1,0),
                        new Points(-1,0),
                        new Points(-1,-1)};

                default:
                    return null;
            }
        }
    }
}
