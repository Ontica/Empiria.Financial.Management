/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Data Layer                              *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Data Service                            *
*  Type     : CashflowProjectionDataService              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for cashflow projections.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Cashflow.Projections.Data {

  /// <summary>Provides data access services for cashflow projections.</summary>
  static internal class CashflowProjectionDataService {

    static internal string GetNextProjectionNo(CashflowProjection projection) {
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


    static internal FixedList<CashflowProjection> SearchProjections(string filter, string sort) {
      Assertion.Require(filter, nameof(filter));
      Assertion.Require(sort, nameof(sort));

      var sql = "SELECT * FROM FMS_CASHFLOW_PROJECTIONS " +
               $"WHERE {filter} " +
               $"ORDER BY {sort}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<CashflowProjection>(op);

    }


    static internal void WriteProjection(CashflowProjection o) {
      var op = DataOperation.Parse("write_FMS_Cashflow_Projection",
          o.Id, o.UID, o.ProjectionType.Id, o.Category.Id, o.Plan.Id, o.ProjectionNo,
          o.BaseParty.Id, o.BaseProject.Id, o.BaseAccount.Id, o.Source.Id, o.Description,
          o.Justification, o.Identificators, o.Tags, o.AttributesData.ToString(),
          o.FinancialData.ToString(), o.ConfigData.ToString(), o.ExtData.ToString(),
          o.ApplicationDate, o.AppliedBy.Id, o.RecordingTime, o.RecordedBy.Id,
          o.AuthorizationTime, o.AuthorizedBy.Id, o.RequestedTime, o.RequestedBy.Id,
          o.Keywords, o.AdjustementOf.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class CashflowProjectionDataService

}  // namespace Empiria.Cashflow.Projections.Data
