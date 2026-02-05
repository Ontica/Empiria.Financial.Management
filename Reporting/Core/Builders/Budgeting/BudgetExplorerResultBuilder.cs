/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetExplorerResultBuilder                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with the results of a budget explorer request.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;
using Empiria.DynamicData;

using Empiria.Budgeting.Explorer.Adapters;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with the results of a budget explorer request.</summary>
  internal class BudgetExplorerResultBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public BudgetExplorerResultBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(BudgetExplorerResultDto budgetExplorerResult) {
      Assertion.Require(budgetExplorerResult, nameof(budgetExplorerResult));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader(budgetExplorerResult.Columns);

      FillOut(budgetExplorerResult);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader(FixedList<DataTableColumn> columns) {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(BudgetExplorerResultDto explorerResult) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in explorerResult.Entries) {

        if (explorerResult.Query.GroupByColumn == Explorer.BudgetExplorerGroupBy.AREA_PARTIDA) {
          _excelFile.SetCell($"A{i}", entry.OrganizationalUnitName);
        } else {
          _excelFile.SetCell($"A{i}", "BANOBRAS");
        }
        _excelFile.SetCell($"B{i}", entry.BudgetAccountName);
        _excelFile.SetCell($"C{i}", entry.MonthName);
        _excelFile.SetCell($"D{i}", entry.Planned);
        _excelFile.SetCell($"E{i}", entry.Authorized);
        _excelFile.SetCell($"F{i}", entry.Expanded);
        _excelFile.SetCell($"G{i}", entry.Reduced);
        _excelFile.SetCell($"H{i}", entry.Modified);
        _excelFile.SetCell($"I{i}", entry.Requested);
        _excelFile.SetCell($"J{i}", entry.Commited);
        _excelFile.SetCell($"K{i}", entry.ToPay);
        _excelFile.SetCell($"L{i}", entry.Excercised);
        _excelFile.SetCell($"M{i}", entry.ToExercise);
        _excelFile.SetCell($"N{i}", entry.Available);

        i++;

      }  //  foreach entry
    }

  } // class BudgetExplorerResultBuilder

} // namespace Empiria.Budgeting.Reporting
