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
         "method":"get",
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
         "method":"json",
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
