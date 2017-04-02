function validateperson(id)
{
    var valid = true;
    var p ="#" +id;
    if(id=='p')
        p='#';
     
    if ($(p + "newname").val().length == 0)
    {
        $("#newname" + id).show();
        valid= false;
    }
    else {

        $("#newname" + id).hide();
    }
        
    if ($(p + "newsex").val().length == 0 ||( $(p + "newsex").val().toLowerCase() != "male" && $(p + "newsex").val().toLowerCase() != "female" && $(p + "newsex").val().toLowerCase() != "other") ){
        $("#newsex" + id).show();
        valid= false;
    }
    else {
        $("#newsex" + id).hide();

    }
    var d = new Date($(p + "newdob").val()).toDateString();
    console.log(d);
    console.log($(p + "newdob").val())
    if (($(p + "newdob").val().length == 0) || ($(p + "newdob").val() == "0001-01-01")) {
        $("#newdob" + id).show();
       valid = false;
    }
       
    else {
        $("#newdob" + id).hide();
    }
    return valid;

}

