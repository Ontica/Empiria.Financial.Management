/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Data Layer                              *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Data Service                            *
*  Type     : CashFlowProjectionDataService              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for cash flow projections.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.CashFlow.Projections.Data {

  /// <summary>Provides data access services for cash flow projections.</summary>
  static internal class CashFlowProjectionDataService {

    static internal string GetNextProjectionNo(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      if (projection.HasProjectionNo) {
        return projection.ProjectionNo;
      }

      string prefix = projection.Plan.Prefix;

      string sql = "SELECT MAX(CFW_PJC_NO) " +
                   "FROM FMS_CASHFLOW_PROJECTIONS " +
                   $"WHERE CFW_PJC_NO LIKE '{prefix}-%'";

      string lastUniqueID = DataReader.GetScalar(DataOperation.Parse(sql), string.Empty);

      if (lastUniqueID.Length != 0) {

        int consecutive = int.Parse(lastUniqueID.Substring(lastUniqueID.LastIndexOf('-') + 1));

        return $"{prefix}-{consecutive:00000}";

      } else {
        return $"{prefix}-00001";
      }
    }

    static internal List<CashFlowProjection> GetProjections(CashFlowPlan cashFlowPlan) {
      Assertion.Require(cashFlowPlan, nameof(cashFlowPlan));

      if (cashFlowPlan.IsEmptyInstance) {
        return new List<CashFlowProjection>() ;
      }

      var sql = "SELECT * FROM FMS_CASHFLOW_PROJECTIONS " +
               $"WHERE CFW_PJC_PLAN_ID = {cashFlowPlan.Id} AND " +
               $"CFW_PJC_STATUS <> 'X' " +
               $"ORDER BY CFW_PJC_NO";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<CashFlowProjection>(op);
    }


    static internal List<CashFlowProjectionEntry> GetProjectionEntries(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      var sql = "SELECT * FROM FMS_CASHFLOW_PJC_ENTRIES " +
                $"WHERE CFW_PJC_ENTRY_PJC_ID = {projection.Id} AND " +
                $"CFW_PJC_ENTRY_STATUS <> 'X' " +
                $"ORDER BY CFW_PJC_ENTRY_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<CashFlowProjectionEntry>(op);
    }


    static internal FixedList<CashFlowProjection> SearchProjections(string filter, string sort) {
      Assertion.Require(filter, nameof(filter));
      Assertion.Require(sort, nameof(sort));

      var sql = "SELECT * FROM FMS_CASHFLOW_PROJECTIONS " +
               $"WHERE {filter} " +
               $"ORDER BY {sort}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<CashFlowProjection>(op);
    }


    static internal void WriteProjection(CashFlowProjection o) {
      var op = DataOperation.Parse("write_FMS_CashFlow_Projection",
          o.Id, o.UID, o.ProjectionType.Id, o.Category.Id, o.Plan.Id, o.ProjectionNo,
          o.BaseParty.Id, o.BaseProject.Id, o.BaseAccount.Id, o.OperationSource.Id,
          o.Description, o.Justification, o.Identificators, o.Tags, o.AttributesData.ToString(),
          o.FinancialData.ToString(), o.ConfigData.ToString(), o.ExtData.ToString(),
          o.ApplicationDate, o.AppliedBy.Id, o.RecordingTime, o.RecordedBy.Id,
          o.AuthorizationTime, o.AuthorizedBy.Id, o.RequestedTime, o.RequestedBy.Id,
          o.Keywords, o.AdjustmentOf.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteProjectionEntry(CashFlowProjectionEntry o) {
      var op = DataOperation.Parse("write_FMS_CashFlow_Pjc_Entry",
          o.Id, o.UID, o.GetEmpiriaType().Id, o.Projection.Id, o.CashFlowAccount.Id, o.OperationTypeId,
          o.FinancialProject.Id, o.Party.Id, o.Product.Id, o.ProductUnit.Id, o.ProductQty,
          o.Year, o.Month, o.Day, o.ProjectionColumn.Id, o.Currency.Id, o.OriginalAmount,
          o.InflowAmount, o.OutflowAmount, o.ExchangeRate, o.Description, o.Justification,
          string.Join(" ", o.Tags), o.ExtensionData.ToString(), o.Keywords, o.LinkedProjectionEntryId,
          o.Position, o.PostingTime, o.PostedBy.Id, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class CashFlowProjectionDataService

}  // namespace Empiria.CashFlow.Projections.Data
