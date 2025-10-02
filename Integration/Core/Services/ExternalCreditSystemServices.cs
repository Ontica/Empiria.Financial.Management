/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Service provider                        *
*  Type     : ExternalCreditSystemServices               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services from external credit systems.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Reflection;

using Empiria.Financial.Adapters;

namespace Empiria.Financial {

  /// <summary>Provides services from external credit systems.</summary>
  public class ExternalCreditSystemServices {

    #region Fields

    private readonly ICreditAccountService _service;

    #endregion Fields

    #region Constructors and parsers

    public ExternalCreditSystemServices() {
      var typeConfig = ConfigurationData.GetString("ExternalCreditSystemServices");

      string[] typeData = typeConfig.Split(';');

      Type type = ObjectFactory.GetType(assemblyName: typeData[0],
                                        typeName: typeData[1]);

      _service = (ICreditAccountService) ObjectFactory.CreateObject(type);
    }

    #endregion Constructors and parsers

    #region Methods

    public FixedList<ICreditEntryData> GetCreditEntries(FixedList<string> creditIDs,
                                                        DateTime fromDate,
                                                        DateTime toDate) {
      Assertion.Require(creditIDs, nameof(creditIDs));
      Assertion.Require(fromDate, nameof(fromDate));
      Assertion.Require(toDate, nameof(toDate));

      return _service.GetCreditsEntries(creditIDs, fromDate, toDate);
    }


    public ICreditAccountData TryGetCreditWithAccountNo(string creditNo) {
      Assertion.Require(creditNo, nameof(creditNo));

      return _service.TryGetCredit(creditNo);
    }

    #endregion Methods

  }  // ExternalCreditSystemServices

}  // namespace Empiria.Financial.Adapters
