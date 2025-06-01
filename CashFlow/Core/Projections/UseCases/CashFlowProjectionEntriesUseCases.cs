/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : CashFlowProjectionEntriesUseCases          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve and update cash flow projections entries.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.CashFlow.Projections.Adapters;
using System;

namespace Empiria.CashFlow.Projections.UseCases {

  /// <summary>Use cases used to retrieve and update cash flow projections entries.</summary>
  public class CashFlowProjectionEntriesUseCases : UseCase {

    #region Constructors and parsers

    protected CashFlowProjectionEntriesUseCases() {
      // no-op
    }

    static public CashFlowProjectionEntriesUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowProjectionEntriesUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public CashFlowProjectionEntryDto CreateProjectionEntry(CashFlowProjectionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      CashFlowProjectionEntry entry = projection.AddEntry(fields);

      projection.Save();

      return CashFlowProjectionEntryMapper.Map(entry);
    }


    public CashFlowProjectionEntryDto GetProjectionEntry(CashFlowProjectionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      CashFlowProjectionEntry entry = projection.GetEntry(fields.UID);

      return CashFlowProjectionEntryMapper.Map(entry);
    }


    public CashFlowProjectionEntryDto RemoveProjectionEntry(CashFlowProjectionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      CashFlowProjectionEntry entry = projection.GetEntry(fields.UID);

      projection.RemoveEntry(entry);

      projection.Save();

      return CashFlowProjectionEntryMapper.Map(entry);
    }


    public CashFlowProjectionEntryDto UpdateProjectionEntry(CashFlowProjectionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      CashFlowProjectionEntry entry = projection.GetEntry(fields.UID);

      projection.UpdateEntry(entry, fields);

      projection.Save();

      return CashFlowProjectionEntryMapper.Map(entry);
    }

    #endregion Use cases

  }  // class CashFlowProjectionEntriesUseCases

}  // namespace Empiria.CashFlow.Projections.UseCases
