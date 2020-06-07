// htm.core C# REST examples
//
// Copyright(C) 2020, Martin Kandlbinder, https://3rdman.de
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY - without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading;

namespace UpDownSequence
{
    /// <summary>
    /// Display the result of one NetworkAPI iteration.
    /// Creates a header showing the current cycle, step, value and anomaly score in a header
    /// and prints each cell with its own foreground and background color, depending on its state (active, winner and/or predictive)
    /// </summary>
    internal static class DisplayOutput
    {
        /// <summary>
        /// Prints the overall result to the console for a certain amount of time.
        /// </summary>
        /// <param name="result"></param>
        internal static void PrintResult(DisplayData result)
        {
            PrintHeader(result);
            for (int rowIdx = 0; rowIdx < ExampleConfiguration.CellsPerColumn; rowIdx++)
            {
                PrintCellRow(result, rowIdx);
            }
            Console.WriteLine();

            Thread.Sleep(ExampleConfiguration.DisplaySleepMilliseconds);
        }

        private static void PrintCellRow(DisplayData result, int rowIdx)
        {
            Console.Write("| Cell    |");
            for (int colIdx = 0; colIdx < ExampleConfiguration.ColumnSize; colIdx++)
            {
                int cellId = colIdx * ExampleConfiguration.CellsPerColumn + rowIdx;
                byte activeState = result.ActiveSdr.GetDense()[cellId];
                byte winnerState = result.WinnderSdr.GetDense()[cellId];
                byte predictiveState = result.PredictiveSdr.GetDense()[cellId];
                PrintCell(cellId, activeState, winnerState, predictiveState);
            }
            Console.WriteLine();
        }

        private static void PrintHeader(DisplayData result)
        {
            Console.Clear();
            PrintSeparatorLine();
            Console.WriteLine($"| Cycle: {result.CycleIdx:D2} | Step: {result.StepIdx:D2} | Value: {result.Value:D2} | Anomaly: {result.AnomalyScore:0.0} |");
            PrintSeparatorLine();
            PrintColumnInfo();
            PrintSeparatorLine();
        }

        private static void PrintColumnInfo()
        {
            Console.Write("| Column  |");
            for (int colId = 0; colId < ExampleConfiguration.ColumnSize; colId++)
            {
                Console.Write($" {colId:D2} |");
            }
            Console.WriteLine();
        }

        private static void PrintSeparatorLine()
        {
            int charMultiplier = 51;
            Console.WriteLine($"{new string('-', charMultiplier)}");
        }

        private static void PrintCell(int cellId, byte activeState, byte winnerState, byte predictiveState)
        {
            SetDefaultColors();
            Console.Write(" ");

            SetCellColors(activeState, winnerState, predictiveState);

            Console.Write($"{cellId:D2}");
            SetDefaultColors();
            Console.Write(" |");
        }

        /// <summary>
        /// Sets the console foreground and background color based on the cell states
        /// </summary>
        /// <param name="activeState"></param>
        /// <param name="winnerState"></param>
        /// <param name="predictiveState"></param>
        private static void SetCellColors(byte activeState, byte winnerState, byte predictiveState)
        {
            if (activeState == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            if (winnerState == 1)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }

            if (predictiveState == 1)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }
        }

        /// <summary>
        /// Sets the default foreground and background color
        /// </summary>
        private static void SetDefaultColors()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
