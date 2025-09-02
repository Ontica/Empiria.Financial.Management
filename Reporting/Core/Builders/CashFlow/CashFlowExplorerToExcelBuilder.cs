/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : CashFlowExplorerToExcelBuilder                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with cash flow explorer results.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Reporting {

  /// <summary>Builds an Excel file with cash flow explorer results.</summary>
  internal class CashFlowExplorerToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public CashFlowExplorerToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(CashFlowExplorerResultDto explorerResult) {
      Assertion.Require(explorerResult, nameof(explorerResult));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(explorerResult);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(CashFlowExplorerResultDto explorerResult) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in explorerResult.Entries) {

        _excelFile.SetCell($"A{i}", entry.CashAccountNo);
        _excelFile.SetCell($"B{i}", entry.ConceptDescription);
        _excelFile.SetCell($"C{i}", entry.Program);
        _excelFile.SetCell($"D{i}", entry.Subprogram);
        _excelFile.SetCell($"E{i}", entry.FinancingSource);
        _excelFile.SetCell($"F{i}", entry.OperationType);
        _excelFile.SetCell($"G{i}", entry.CurrencyCode);
        _excelFile.SetCell($"H{i}", entry.Inflows);
        _excelFile.SetCell($"I{i}", entry.Outflows);

        i++;

      }  //  foreach entry
    }

  } // class CashFlowExplorerToExcelBuilder

} // namespace Empiria.CashFlow.Reporting
