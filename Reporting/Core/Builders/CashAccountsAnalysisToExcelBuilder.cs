/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : CashAccountsAnalysisToExcelBuilder            License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with cash ledger accounts analysis.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;

using Empiria.Office;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.Financial.Reporting {

  /// <summary>Builds an Excel file with cash ledger accounts analysis.</summary>
  internal class CashAccountsAnalysisToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public CashAccountsAnalysisToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<CashTransactionEntryDto> entries) {
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


    private void FillOut(FixedList<CashTransactionEntryDto> entries) {
      int DEFAULT_CURRENCY_ID = Currency.Default.Id;

      int i = _templateConfig.FirstRowIndex;

      var groups = entries.GroupBy(x => $"{x.CashAccountNo}|{x.CuentaSistemaLegado}|{x.CurrencyId}|{x.AccountNumber}")
                          .OrderBy(x => x.Key);

      foreach (var group in groups) {
        var pivot = group.First();

        _excelFile.SetCell($"A{i}", pivot.CashAccountNo);
        _excelFile.SetCell($"B{i}", pivot.CuentaSistemaLegado);

        _excelFile.SetCell($"C{i}", group.Count());

        _excelFile.SetCellIfValue($"D{i}", group.Count(x => x.CashAccountNo == "Pendiente"));

        _excelFile.SetCellIfValue($"E{i}", group.Count(x => x.CashAccountNo == x.CuentaSistemaLegado));

        _excelFile.SetCellIfValue($"F{i}", group.Count(x => x.CashAccountNo == "Pendiente" ||
                                                            (x.CashAccountNo == "Con flujo" && x.CuentaSistemaLegado != "Sin flujo")));

        _excelFile.SetCellIfValue($"G{i}", group.Count(x => (x.CashAccountNo == "Con flujo" && x.CuentaSistemaLegado == "Sin flujo") ||
                                                            (x.CashAccountNo == "Sin flujo" && x.CuentaSistemaLegado != "Sin flujo") ||
                                                            (x.CashAccountId > 0 && x.CashAccountNo != x.CuentaSistemaLegado)));

        _excelFile.SetCell($"H{i}", pivot.CurrencyName);
        _excelFile.SetCellIfValue($"I{i}", group.Sum(x => x.Debit));
        _excelFile.SetCellIfValue($"J{i}", group.Sum(x => x.Credit));
        _excelFile.SetCell($"K{i}", pivot.AccountNumber);
        _excelFile.SetCell($"L{i}", pivot.AccountName);
        _excelFile.SetCell($"M{i}", pivot.ParentAccountFullName);

        var rules = group.ToFixedList()
                          .FindAll(x => (x.CashAccountNo == "Con flujo" && x.CuentaSistemaLegado == "Sin flujo") ||
                                        (x.CashAccountNo == "Sin flujo" && x.CuentaSistemaLegado != "Sin flujo") ||
                                        (x.CashAccountId > 0 && x.CashAccountNo != x.CuentaSistemaLegado))
                          .SelectDistinct(x => x.CashAccountAppliedRule)
                          .ToArray();


        _excelFile.SetCell($"N{i}", string.Join("; ", rules));

        i++;
      }

    }

  } // class CashAccountsAnalysisToExcelBuilder

} // namespace Empiria.Financial.Reporting
