/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : FinancialRuleToExcelBuilder                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with a given list of category rules.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.DynamicData;
using Empiria.Financial.Rules.Adapters;
using Empiria.Office;
using Empiria.Storage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Empiria.Financial.Rules {

  /// <summary>Builds an Excel file with a given list of category rules.</summary>
  internal class FinancialRuleToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public FinancialRuleToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<FinancialRuleDescriptor> entries) {
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


    private void FillOut(FixedList<FinancialRuleDescriptor> entries) {
      int i = _templateConfig.FirstRowIndex;
      
         foreach (var entry in entries) {

        _excelFile.SetCell($"A{i}", entry.CreditAccount);
        _excelFile.SetCell($"B{i}", entry.CreditConcept);
        _excelFile.SetCell($"C{i}", entry.Description);
        _excelFile.SetCell($"D{i}", entry.StartDate);
        _excelFile.SetCell($"E{i}", entry.EndDate);
        _excelFile.SetCell($"F{i}", entry.StatusName);
        i++;

      }  // foreach entry

    }

  } // class FinancialRuleToExcelBuilder

} // namespace Empiria.Financial.Rules
