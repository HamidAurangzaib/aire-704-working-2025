using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aire
{
    /// <summary>
    /// Helper class for applying target categorization colors to GoogleAirline DataGridView
    /// </summary>
    public static class GoogleAirlineTargetColorHelper
    {
        /// <summary>
        /// Applies color coding to DataGridView rows based on target categorization
        /// Color Priority (highest to lowest):
        /// 1. Green (TargetDeal) - Cheapest deals
        /// 2. Purple (IsMonthTarget) - Month-based targets
        /// 3. Yellow (IsOldTarget) - Old targets (Difference -5 to 0)
        /// 4. Blue (IsTargetFound) - Regular targets
        /// 5. White (Default) - Non-target records
        /// </summary>
        /// <param name="dataGridView">The DataGridView to color</param>
        /// <param name="isOldTargetColumnIndex">Column index for IsOldTarget (or -1 if not available)</param>
        /// <param name="isMonthTargetColumnIndex">Column index for IsMonthTarget (or -1 if not available)</param>
        /// <param name="targetDealColumnIndex">Column index for TargetDeal (or -1 if not available)</param>
        /// <param name="isTargetFoundColumnIndex">Column index for IsTargetFound (or -1 if not available)</param>
        public static async Task ApplyTargetCategorizationColors(
            DataGridView dataGridView,
            int isOldTargetColumnIndex = -1,
            int isMonthTargetColumnIndex = -1,
            int targetDealColumnIndex = -1,
            int isTargetFoundColumnIndex = -1)
        {
            await Task.Run(() =>
            {
                if (dataGridView.InvokeRequired)
                {
                    dataGridView.Invoke(new Action(() =>
                    {
                        ApplyColorsInternal(dataGridView, isOldTargetColumnIndex, isMonthTargetColumnIndex, 
                            targetDealColumnIndex, isTargetFoundColumnIndex);
                    }));
                }
                else
                {
                    ApplyColorsInternal(dataGridView, isOldTargetColumnIndex, isMonthTargetColumnIndex, 
                        targetDealColumnIndex, isTargetFoundColumnIndex);
                }
            });
        }

        private static void ApplyColorsInternal(
            DataGridView dataGridView,
            int isOldTargetColumnIndex,
            int isMonthTargetColumnIndex,
            int targetDealColumnIndex,
            int isTargetFoundColumnIndex)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    bool isOldTarget = false;
                    bool isMonthTarget = false;
                    bool targetDeal = false;
                    bool isTargetFound = false;

                    // Read values from cells if column indices are provided
                    if (isOldTargetColumnIndex >= 0 && row.Cells[isOldTargetColumnIndex].Value != null)
                    {
                        bool.TryParse(row.Cells[isOldTargetColumnIndex].Value.ToString(), out isOldTarget);
                    }

                    if (isMonthTargetColumnIndex >= 0 && row.Cells[isMonthTargetColumnIndex].Value != null)
                    {
                        bool.TryParse(row.Cells[isMonthTargetColumnIndex].Value.ToString(), out isMonthTarget);
                    }

                    if (targetDealColumnIndex >= 0 && row.Cells[targetDealColumnIndex].Value != null)
                    {
                        bool.TryParse(row.Cells[targetDealColumnIndex].Value.ToString(), out targetDeal);
                    }

                    if (isTargetFoundColumnIndex >= 0 && row.Cells[isTargetFoundColumnIndex].Value != null)
                    {
                        bool.TryParse(row.Cells[isTargetFoundColumnIndex].Value.ToString(), out isTargetFound);
                    }

                    // Apply color based on priority
                    Color rowColor = GetTargetCategoryColor(isOldTarget, isMonthTarget, targetDeal, isTargetFound);
                    
                    if (rowColor != Color.White)
                    {
                        row.DefaultCellStyle.BackColor = rowColor;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                System.Diagnostics.Debug.WriteLine($"Error applying target colors: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the appropriate color based on target categorization flags
        /// Priority: Green > Purple > Yellow > Blue > White
        /// </summary>
        private static Color GetTargetCategoryColor(bool isOldTarget, bool isMonthTarget, bool targetDeal, bool isTargetFound)
        {
            if (targetDeal) return Color.LightGreen;  // Green for best deals
            if (isMonthTarget) return Color.MediumPurple;  // Purple for month targets
            if (isOldTarget) return Color.Yellow;  // Yellow for old targets
            if (isTargetFound) return Color.SkyBlue;  // Blue for regular targets
            return Color.White;  // White for regular records
        }

        /// <summary>
        /// Gets a human-readable category name based on flags
        /// </summary>
        public static string GetCategoryName(bool isOldTarget, bool isMonthTarget, bool targetDeal, bool isTargetFound)
        {
            if (targetDeal) return "TargetDeal (Green) - Best Price";
            if (isMonthTarget) return "MonthTarget (Purple) - Different Month";
            if (isOldTarget) return "OldTarget (Yellow) - Difference -5 to 0";
            if (isTargetFound) return "Target (Blue) - Regular Target";
            return "Regular Record";
        }

        /// <summary>
        /// Adds a legend/key to help users understand the color coding
        /// Call this method to add a label explaining the colors
        /// </summary>
        public static Label CreateColorLegend()
        {
            Label legend = new Label();
            legend.AutoSize = true;
            legend.Text = "Color Legend:\n" +
                         "🟢 Green = Best Deals (Cheapest price)\n" +
                         "🟣 Purple = Month Targets (Different months)\n" +
                         "🟡 Yellow = Old Targets (Difference -5 to 0)\n" +
                         "🔵 Blue = Regular Targets\n" +
                         "⚪ White = Non-target records";
            legend.Font = new Font("Arial", 9, FontStyle.Regular);
            return legend;
        }

        /// <summary>
        /// Enhanced version that also colors the Difference cell based on value
        /// </summary>
        public static async Task ApplyEnhancedColors(
            DataGridView dataGridView,
            int differenceColumnIndex,
            int isOldTargetColumnIndex = -1,
            int isMonthTargetColumnIndex = -1,
            int targetDealColumnIndex = -1,
            int isTargetFoundColumnIndex = -1)
        {
            await Task.Run(() =>
            {
                if (dataGridView.InvokeRequired)
                {
                    dataGridView.Invoke(new Action(() =>
                    {
                        ApplyEnhancedColorsInternal(dataGridView, differenceColumnIndex, 
                            isOldTargetColumnIndex, isMonthTargetColumnIndex, 
                            targetDealColumnIndex, isTargetFoundColumnIndex);
                    }));
                }
                else
                {
                    ApplyEnhancedColorsInternal(dataGridView, differenceColumnIndex, 
                        isOldTargetColumnIndex, isMonthTargetColumnIndex, 
                        targetDealColumnIndex, isTargetFoundColumnIndex);
                }
            });
        }

        private static void ApplyEnhancedColorsInternal(
            DataGridView dataGridView,
            int differenceColumnIndex,
            int isOldTargetColumnIndex,
            int isMonthTargetColumnIndex,
            int targetDealColumnIndex,
            int isTargetFoundColumnIndex)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Apply row color based on target categorization
                    bool isOldTarget = false;
                    bool isMonthTarget = false;
                    bool targetDeal = false;
                    bool isTargetFound = false;

                    if (isOldTargetColumnIndex >= 0 && row.Cells[isOldTargetColumnIndex].Value != null)
                        bool.TryParse(row.Cells[isOldTargetColumnIndex].Value.ToString(), out isOldTarget);

                    if (isMonthTargetColumnIndex >= 0 && row.Cells[isMonthTargetColumnIndex].Value != null)
                        bool.TryParse(row.Cells[isMonthTargetColumnIndex].Value.ToString(), out isMonthTarget);

                    if (targetDealColumnIndex >= 0 && row.Cells[targetDealColumnIndex].Value != null)
                        bool.TryParse(row.Cells[targetDealColumnIndex].Value.ToString(), out targetDeal);

                    if (isTargetFoundColumnIndex >= 0 && row.Cells[isTargetFoundColumnIndex].Value != null)
                        bool.TryParse(row.Cells[isTargetFoundColumnIndex].Value.ToString(), out isTargetFound);

                    // Apply row color
                    Color rowColor = GetTargetCategoryColor(isOldTarget, isMonthTarget, targetDeal, isTargetFound);
                    if (rowColor != Color.White)
                    {
                        row.DefaultCellStyle.BackColor = rowColor;
                    }

                    // Apply Difference cell color (negative = green, positive = red, zero = special cases)
                    if (differenceColumnIndex >= 0 && row.Cells[differenceColumnIndex].Value != null)
                    {
                        if (double.TryParse(row.Cells[differenceColumnIndex].Value.ToString(), out double difference))
                        {
                            if (difference < 0)
                            {
                                row.Cells[differenceColumnIndex].Style.BackColor = Color.LightGreen;
                            }
                            else if (difference > 0)
                            {
                                row.Cells[differenceColumnIndex].Style.BackColor = Color.LightCoral;
                            }
                            else // difference == 0
                            {
                                // Keep special case coloring from existing logic if needed
                                row.Cells[differenceColumnIndex].Style.BackColor = Color.LightGray;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error applying enhanced colors: {ex.Message}");
            }
        }
    }
}








