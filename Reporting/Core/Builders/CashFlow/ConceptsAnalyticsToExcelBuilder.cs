/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : ConceptsAnalyticToExcelBuilder                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate concepts analytics by account report.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Reporting {

  /// <summary>Builds an Excel file containing concepts analytics by account report.</summary>
  internal class ConceptsAnalyticsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public ConceptsAnalyticsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(DynamicDto<ConceptAnalyticsDto> entries) {
      Assertion.Require(entries, nameof(entries));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(entries);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(DynamicDto<ConceptAnalyticsDto> entries) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in entries.Entries) {

        _excelFile.SetCell($"A{i}", entry.CashAccountNo);
        _excelFile.SetCell($"B{i}", entry.ConceptDescription);
        _excelFile.SetCell($"C{i}", entry.AccountNumber);
        _excelFile.SetCell($"D{i}", entry.TransactionAccountingDate);
        _excelFile.SetCell($"E{i}", entry.CurrencyName);
        _excelFile.SetCell($"F{i}", entry.ExchangeRate);
        _excelFile.SetCell($"G{i}", entry.Debit);
        _excelFile.SetCell($"H{i}", entry.Credit);
        _excelFile.SetCell($"I{i}", entry.TransactionNumber);
        _excelFile.SetCell($"J{i}", entry.FinancialAcctOrgUnit);
        _excelFile.SetCell($"K{i}", entry.SubledgerAccountNumber);
        _excelFile.SetCell($"L{i}", entry.ProjectType);
        _excelFile.SetCell($"M{i}", entry.FinancialAcctType);
        _excelFile.SetCell($"N{i}", entry.FinancialAcctName);

        i++;

      }  //  foreach entry
    }

  }  // class ConceptsAnalyticsToExcelBuilder

}  // namespace Empiria.CashFlow.Reporting
