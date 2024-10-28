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
      var sql = "SELECT * FROM FMS_BUDGET_ACCOUNTS_SEGMENTS " +
                $"WHERE BDG_ACCT_SEGMENT_TYPE_ID = {segmentType.Id} " +
                "AND BDG_ACCT_SEGMENT_STATUS <> 'X' " +
                $"ORDER BY BDG_ACCT_SEGMENT_CODE";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetAccountSegment>(op);
    }


    static internal FixedList<BudgetAccountSegment> BudgetAccountChildrenSegments(BudgetAccountSegment parentSegment) {
      var sql = "SELECT * FROM FMS_BUDGET_ACCOUNTS_SEGMENTS " +
                $"WHERE BDG_ACCT_SEGMENT_PARENT_ID = {parentSegment.Id} " +
                "AND BDG_ACCT_SEGMENT_STATUS <> 'X' " +
                $"ORDER BY BDG_ACCT_SEGMENT_CODE";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetAccountSegment>(op);
    }


    static internal void Write(BudgetAccountSegment o) {
      var op = DataOperation.Parse("WRITE_FMS_BUDGET_ACCOUNT_SEGMENT",
                          o.Id, o.UID, o.BudgetSegmentType.Id,
                          o.Code, o.Name, o.Description, o.ExternalObjectReferenceId,
                          o.ExtensionData.ToString(), o.Keywords, o.Id,
                          o.StartDate, o.EndDate, o.Parent.Id,
                          o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class BudgetAccountSegmentDataService

}  // namespace Empiria.Budgeting.Data
