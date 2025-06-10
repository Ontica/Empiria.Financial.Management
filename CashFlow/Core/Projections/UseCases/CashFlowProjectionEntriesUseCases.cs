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

    #region Single entry use cases

    public FixedList<CashFlowProjectionEntryDto> CalculateProjectionEntries(string projectionUID) {
      Assertion.Require(projectionUID, nameof(projectionUID));

      var projection = CashFlowProjection.Parse(projectionUID);

      return CashFlowProjectionEntryMapper.Map(projection.Entries);
    }


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

    #endregion Single entry use cases

    #region By year entries use cases

    public CashFlowProjectionEntryByYearDto CreateProjectionEntryByYear(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      var byYearProjection = new CashFlowProjectionByYear(projection);

      FixedList<CashFlowProjectionEntry> entries = byYearProjection.CreateProjectionEntries(fields);

      projection.UpdateEntries(entries);

      projection.Save();

      var entryByYear = new CashFlowProjectionEntryByYear(byYearProjection.BuildUID(entries[0]), entries);

      return CashFlowProjectionEntryByYearMapper.Map(entryByYear);
    }


    public CashFlowProjectionEntryByYearDto GetProjectionEntryByYear(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      var byYearProjection = new CashFlowProjectionByYear(projection);

      FixedList<CashFlowProjectionEntry> entries = byYearProjection.GetProjectionEntries(fields.UID);

      var entryByYear = new CashFlowProjectionEntryByYear(fields.UID, entries);

      return CashFlowProjectionEntryByYearMapper.Map(entryByYear);
    }


    public void RemoveProjectionEntryByYear(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      var byYearProjection = new CashFlowProjectionByYear(projection);

      FixedList<CashFlowProjectionEntry> entries = byYearProjection.GetProjectionEntries(fields.UID);

      foreach (var entry in entries) {
        projection.RemoveEntry(entry);
      }

      projection.Save();
    }


    public CashFlowProjectionEntryByYearDto UpdateProjectionEntryByYear(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var projection = CashFlowProjection.Parse(fields.ProjectionUID);

      var byYearProjection = new CashFlowProjectionByYear(projection);

      FixedList<CashFlowProjectionEntry> updatedEntries = byYearProjection.GetUpdatedEntries(fields);

      projection.UpdateEntries(updatedEntries);

      projection.Save();

      var entryByYear = new CashFlowProjectionEntryByYear(fields.UID, updatedEntries);

      return CashFlowProjectionEntryByYearMapper.Map(entryByYear);
    }

    #endregion By year entries use cases

  }  // class CashFlowProjectionEntriesUseCases

}  // namespace Empiria.CashFlow.Projections.UseCases
