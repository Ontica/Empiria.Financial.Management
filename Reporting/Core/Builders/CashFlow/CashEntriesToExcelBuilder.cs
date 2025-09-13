/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : CashEntriesToExcelBuilder                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with cash ledger transaction entries.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Financial;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Reporting {

  /// <summary>Builds an Excel file with cash ledger transaction entries.</summary>
  internal class CashEntriesToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public CashEntriesToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<CashEntryExtendedDto> entries) {
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


    private void FillOut(FixedList<CashEntryExtendedDto> entries) {
      int DEFAULT_CURRENCY_ID = Currency.Default.Id;

      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in entries) {

        _excelFile.SetCell($"A{i}", entry.TransactionLedgerName);
        _excelFile.SetCell($"B{i}", entry.TransactionNumber);
        _excelFile.SetCell($"C{i}", entry.TransactionAccountingDate);
        _excelFile.SetCell($"D{i}", entry.TransactionRecordingDate);
        _excelFile.SetCell($"E{i}", entry.TransactionId);
        _excelFile.SetCell($"F{i}", entry.TransactionConcept);
        _excelFile.SetCell($"G{i}", entry.AccountNumber);
        _excelFile.SetCell($"H{i}", entry.AccountName);
        _excelFile.SetCell($"I{i}", entry.SubledgerAccountNumber);
        _excelFile.SetCell($"J{i}", entry.SubledgerAccountName);
        _excelFile.SetCell($"K{i}", entry.SectorCode);
        _excelFile.SetCell($"L{i}", entry.VerificationNumber);
        _excelFile.SetCell($"M{i}", entry.ResponsibilityAreaCode);
        _excelFile.SetCell($"N{i}", entry.CurrencyName);

        if (entry.CurrencyId != DEFAULT_CURRENCY_ID) {
          _excelFile.SetCell($"O{i}", entry.ExchangeRate);
        }

        _excelFile.SetCellIfValue($"P{i}", entry.Debit);
        _excelFile.SetCellIfValue($"Q{i}", entry.Credit);

        _excelFile.SetCell($"R{i}", entry.CashAccountNo);
        _excelFile.SetCell($"S{i}", entry.CuentaSistemaLegado);

        _excelFile.SetCell($"T{i}", 1);

        _excelFile.SetCellIfValue($"U{i}", entry.Pending ? 1 : 0);
        _excelFile.SetCellIfValue($"V{i}", entry.Exact ? 1 : 0);
        _excelFile.SetCellIfValue($"W{i}", entry.Correct ? 1 : 0);
        _excelFile.SetCellIfValue($"W{i}", entry.FalsePositive ? 1 : 0);

        _excelFile.SetCell($"Y{i}", entry.CashAccountAppliedRule);

        i++;

      }  //  foreach entry

    }

  } // class CashEntriesToExcelBuilder

} // namespace Empiria.CashFlow.Reporting
