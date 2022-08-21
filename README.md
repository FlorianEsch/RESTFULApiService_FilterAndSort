# RESTFULApiService_flaschenpost

the Uri I used: https://flapotest.blob.core.windows.net/test/ProductData.json

We have four different API routes():


/Articles/MinAndMaxPricePerLiter?url=&isMinAndMaxPricePerLiter=true
/Articles/ByPriceAndSortAscending?url=&price=17.99&isSortByAscending=true   
/Articles/MostUnits?url=&isMostUnits=true
/Articles/GetAll?url=&price=17.99&isMinAndMaxPricePerLiter=true&isMostUnits=true&isSortByAscending=true


The Api is Deserializing a List<Product> 
and Creating ArticleContext Objects.
They can be sorted and filtered by filling the Attributes;


I have also Created a html View to make the testing easier.
You can choose what kind of filter you want.
If you click on the "Get" Button, it will bring you to the Output Page.
