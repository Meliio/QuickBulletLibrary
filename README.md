# QuickBulletLibrary

### Config example
```json
{
   "settings":{
      "name":"ipify",
      "inputSeparator":"",
      "inputNames":[
         
      ]
   },
   "blocks":[
      {
         "block":"request",
         "url":"https://api.ipify.org?format=json",
         "methode":"get",
         "data":"",
         "contentType":"",
         "headers":[

         ],
         "cookies":[
            
         ],
         "loadContent":true,
         "isDisable":false
      },
      {
         "block":"parse",
         "name":"IP",
         "prefix":"",
         "suffix":"",
         "source":"{request.Content}",
         "methode":"json",
         "firstInput":"ip",
         "secondInput":"",
         "addToOutput":true,
         "isDisable":false
      },
      {
         "block":"condition",
         "patterns":[
            {
               "status":"success",
               "source":"{request.StatusCode}",
               "condition":"equal",
               "key":"200"
            }
         ],
         "isDisable":false
      }
   ]
}
```
