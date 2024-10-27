/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Data Service                            *
*  Type     : BudgetAccountSegmentDataService            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for budget account segments.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Budgeting.Data {

  /// <summary>Provides data access services for budget account segments.</summary>
  static internal class BudgetAccountSegmentDataService {

    static internal FixedList<BudgetAccountSegment> BudgetAccountSegments(BudgetAccountSegmentType segmentType) {
      var sql = "SELECT * FROM BDG_SEGMENT_ITEMS " +
                $"WHERE BDG_SEG_TYPE_ID = {segmentType.Id} " +
                "AND BDG_SEG_ITEM_STATUS <> 'X' " +
                $"ORDER BY BDG_SEG_ITEM_CODE";

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetAccountSegment>(dataOperation);
    }


    static internal FixedList<BudgetAccountSegment> BudgetAccountChildrenSegments(BudgetAccountSegment parentSegment) {
      var sql = "SELECT * FROM BDG_SEGMENT_ITEMS " +
                $"WHERE BDG_SEG_ITEM_PARENT_ID = {parentSegment.Id} " +
                "AND BDG_SEG_ITEM_STATUS <> 'X' " +
                $"ORDER BY BDG_SEG_ITEM_CODE";

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetAccountSegment>(dataOperation);
    }


    static internal void Write(BudgetAccountSegment o) {
      var op = DataOperation.Parse("write_BDG_SEGMENT_ITEM",
                          o.Id, o.UID, o.BudgetSegmentType.Id,
                          o.Code, o.Name, o.Description, o.ExternalObjectReferenceId,
                          o.ExtensionData.ToString(), o.Keywords, o.Id,
                          o.StartDate, o.EndDate, o.Parent.Id,
                          o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataReader.GetFixedList<BudgetAccountSegment>(op);
    }

  }  // class BudgetAccountSegmentDataService

}  // namespace Empiria.Budgeting.Data
