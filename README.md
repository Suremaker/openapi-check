# openapi-check

Compares two swagger files (OAS 3) and reports breaking changes.

Usage `openapi-check [swagger-v1.json] [swagger-v2.json]` where files can be referenced by local path or remote URI.

# Comparison rules

## Operations

|action|example|
|--|--|
|✔️ adding new operation| |
|❌ removing operation|`Get /api/Operations/{0}/summary: [Error] Operation no longer exists` |
|⚠️ removing deprecated operation|`[deprecated] Get /api/Operations/some: [Warning] Operation no longer exists` |

## Request

|action|example|
|--|--|
|✔️ adding new request content type| |
|❌ removing request content type|`Post /api/RequestModel: [Error] Operation no longer accepts request for application/xml` |

## Request body

|action|example|
|--|--|
|✔️ making body optional| |
|❌ making body required|`Post /api/RequestModel: [Error] Operation request body is now required` |
|✔️ making field optional| |
|❌ making field required|`Post /api/RequestModel: [Error] request(application/json).body.text is now required` |
|✔️ making field nullable| |
|❌ making field not-nullable| `Post /api/RequestModel: [Error] request(application/json).body.requestId is no longer nullable` |
|✔️ adding new optional field| |
|⚠️ removing deprecated field| `Post /api/RequestBreakingModel: [Warning] request(application/json).body.obsoleteField no longer exists (deprecated)` |
|❌ removing field| `Post /api/RequestModel: [Error] request(application/json).body.additionalParameter no longer exists` |
|❌ changing field/body type| `Post /api/RequestBreakingModel: [Error] request(application/json).body.someField type does not match (before: string, after: integer)` |

## Response

|action|example|
|--|--|
|✔️ adding new response status code| |
|⚠️ removing response status code|`Get /api/Responses: [Warning] Operation no longer returns HTTP 400 code` |
|✔️ adding new response content type| |
|❌ removing response content type|`Get /api/ResponseContent: [Error] Operation no longer returns application/xml for 200 code` |

## Response body

|action|example|
|--|--|
|✔️ making field not-nullable| |
|❌ making field nullable| `Get /api/ResponseModel: [Error] response(HTTP 200\|application/json).body[].text is now nullable` |
|✔️ adding new field| |
|⚠️ removing deprecated field| `Get /api/ResponseBreakingModel/details: [Warning] response(HTTP 200\|application/json).body.obsolete no longer exists (deprecated)` |
|❌ removing field| `Get /api/ResponseModel: [Error] response(HTTP 200\|application/json).body[].newField no longer exists` |
|❌ changing field/body type| `Get /api/ResponseBreakingModel: [Error] response(HTTP 200\|application/json).body type does not match (before: string, after: array)` |
