/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillImporterUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to import bills from xml data structures using SAT Mexico's standard.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Billing.Adapters;

namespace Empiria.Billing.UseCases {

  /// <summary>Use cases to import bills from xml data structures using SAT Mexico's standard.</summary>
  public class BillImporterUseCases : UseCase {

    #region Constructors and parsers


    protected BillImporterUseCases() {
      // no-op
    }

    static public BillImporterUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BillImporterUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public BillDto ImportXmlBillFromFilePath(string xmlFilePath) {

      var billDto = BillXmlReader.ReadFromFilePath(xmlFilePath);

      // Code to store BillDto as Bill

      return billDto;
    }

    #endregion Use cases

  } // class BillImporterUseCases

} // namespace Empiria.Billing.UseCases
