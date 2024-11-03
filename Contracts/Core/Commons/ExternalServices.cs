/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : External Services Layer                 *
*  Assembly : Empiria.Contract.Core.dll                  Pattern   : Service connector                       *
*  Type     : ExternalServices                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Connect Empiria Contracts with external services providers.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.Documents.Services;
using Empiria.Documents.Services.Adapters;

using Empiria.History.Services;
using Empiria.History.Services.Adapters;
using Empiria.Storage;

namespace Empiria.Contracts {

  /// <summary>Connect Empiria Contracts with external services providers.</summary>
  static internal class ExternalServices {

    static internal DocumentDto DeleteDocument(string documentUID) {

      using (var services = DocumentServices.ServiceInteractor()) {
        return services.DeleteDocument(documentUID);
      }
    }


    static internal FixedList<DocumentDto> GetEntityDocuments(BaseObject entity) {
      Assertion.Require(entity, nameof(entity));

      using (var services = DocumentServices.ServiceInteractor()) {

        return services.GetEntityDocuments(entity);
      }
    }


    static internal FixedList<HistoryDto> GetEntityHistory(BaseObject entity) {
      Assertion.Require(entity, nameof(entity));

      using (var services = HistoryServices.ServiceInteractor()) {

        return services.GetEntityHistory(entity);
      }
    }


    static internal DocumentDto StoreDocument(BaseObject baseObject, DocumentFields fields, InputFile inputFile) {
      Assertion.Require(baseObject, nameof(baseObject));
      Assertion.Require(inputFile, nameof(inputFile));
      Assertion.Require(fields, nameof(fields));

      using (var services = DocumentServices.ServiceInteractor()) {

        return services.CreateDocument(inputFile, baseObject, fields);
      }
    }
  }  // class ExternalServices

}  // namespace Empiria.Contracts
