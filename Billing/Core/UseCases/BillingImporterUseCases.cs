/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Billing                          Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillingImporterUseCases                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Build billing models from xml files or URIs.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.IO;
using Empiria.Financial.Management.Billing.Adapters;
using Empiria.Financial.Management.Billing.Domain;
using Empiria.Services;

namespace Empiria.Financial.Management.Billing.UseCases {

  /// <summary>Build billing models from xml files or URIs.</summary>
  public class BillingImporterUseCases : UseCase {

    #region Constructors and parsers


    protected BillingImporterUseCases() {
      // no-op
    }

    static public BillingImporterUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BillingImporterUseCases>();
    }


    #endregion Constructors and parsers


    #region Use cases


    public BillingDto ReadBillingXmlDocument(string xmlPath) {

      return BillingImporterBuilder.ReadBillingXmlDocument(xmlPath);
    }


    #endregion Use cases

  } // class BillingImporterUseCases

} // namespace Empiria.Financial.Management.Billing.UseCases
