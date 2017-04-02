var AddingActor = false;
var AddingProducer = false;
var image;
var actorlist = new Array();
$(document).ready(function () {

               

    setactor =function(value) {
        $("#actors option:contains(" + value + ")").attr('selected', 'selected').change();

                  
    };
    


    $("#poster").change(function () {
        var File = this.files;

        if (File && File[0]) {
            ReadImage(File[0]);
        }


    });

    $("#sub").click(function(){
        if ($("#addnewproducer").is(":visible"))
                addnewproducer();
        if ($("#addnewactor1").is(":visible"))
            addnewactor1();
        if ($("#addnewactor2").is(":visible"))
            addnewactor2();
        if ($("#addnewactor3").is(":visible"))
            addnewactor3();
        //$("#actorsandproducers").attr("disabled",true);
        //$("#sub").attr("disabled",false);
        ////  setactor(actorlist);
        $("#movieform").submit();

    });
    $("#addallactors").click(function () {
        ch = $('#addallactors').is(':checked');
        if (ch) {

            bootbox.confirm("Are you sure you want to add all the actors?", function (result) {
                if (result) {
                  //  @Model.AllActors = true;

                }

            });



        }
        else
        {
                   //@Model.AllActors = false;


        }




    });

    $('#prefillform').click(function () {

                   	
                  
        $("#actors").val("");
        var input = $('#name').val()
        var moviename = encodeURI(input)
        $.ajax({
            url: "/api/Movies/GetMovieDetails?MovieName=" + moviename,
            method: "GET",
            contentType: 'application/json',
            dataType: 'json',
            success: function(data) {
                console.log(data);
                if (data)
                    FillForm(data);
                else
                    bootbox.alert("No such movie!");
            },
            error: function (request, status, error) {
                alert(status + ", " + error);
            }


        });

                    
    });
});
function pad_with_zeroes(number, length) {

    var my_string = '' + number;
    while (my_string.length < length) {
        my_string = '0' + my_string;
    }

    return my_string;

}

formatdate = function (date) {
    var currentTime = new Date(parseInt(date));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var fdate = pad_with_zeroes(year,4) + "-" + pad_with_zeroes(month,2) + "-" + pad_with_zeroes(day,2);
    return fdate;
}

var Existlist = function (name, list) {
    var selectObject = $(list);

    //var actors = @Model.Actors;
    //for (var i=0;i<@Model.Actors.Count;i++) 
    //{
    //    if(actors[i].Name.toLowerCase() === name.toLowerCase())
    //            }*@
       

                for (var i = 0, opts = document.getElementById(list).options; i < opts.length; ++i)
    {
        name = name.toLowerCase();
        act=opts[i].text.toLowerCase();
        if (name === act) {

            return true;

        }

                
    }
    return false;
}
var FillForm = function(data)
{
    //write code to check if the actor already exist before creating individual actors
                
    data = jQuery.parseJSON(data);

    $("#name").val(data.Name);
    $("#plot").val(data.Plot);
    $("#yor").val(data.YearOfRealease);
    // $("#poster").val(data.Poster);
    $("#posterpath").val(data.Poster)


    var producer = data.Producer,
        actlist = [];
    actor1 = data.Actors[0],
    actor2 = (data.Actors[1]),
    actor3 = data.Actors[2];

    if (actor1 && !Existlist(actor1.Name, "actors")) {
        $("#addnewactor1").show();
        $("#a1newname").val(actor1.Name);
        $("#a1newsex").val(actor1.Sex);
        $("#a1newbio").val(actor1.Bio);
        $("#a1newdob").val(formatdate(actor1.Dob.substr(6)));
        //$("#a1newdob").val(actor1.Dob);

    }
    else if(actor1) {
        setactor(actor1.Name);
    }
    if (actor2 && !Existlist(actor2.Name, "actors")) {
        $("#addnewactor2").show();
        $("#a2newname").val(actor2.Name);
        $("#a2newsex").val(actor2.Sex);
        $("#a2newbio").val(actor2.Bio);
        $("#a2newdob").val(formatdate(actor2.Dob.substr(6)));

    }
    else if(actor2) {

        setactor(actor2.Name);

    }
    if (actor3 && !Existlist(actor3.Name, "actors")) {
        $("#addnewactor3").show();
        $("#a3newname").val(actor3.Name);
        $("#a3newsex").val(actor3.Sex);
        $("#a3newbio").val(actor3.Bio);
        $("#a3newdob").val(formatdate(actor3.Dob.substr(6)));
    }
    else if(actor3) {
                    
        setactor(actor3.Name)
                

    }
    if (producer && !Existlist(producer.Name, "producer")) {
        $("#addnewproducer").show();
        $("#newname").val(producer.Name);
        $("#newsex").val(producer.Sex);
        $("#newbio").val(producer.Bio);
        $("#newdob").val(formatdate(producer.Dob.substr(6)));

    }
    else if(producer) {
        $("#producer option:contains(" + producer.Name + ")").attr('selected', 'selected').change();
    }



}

var NewProducer = function () {

    AddingProducer = true;
    //  $("#addnewproducer").load("@Url.Action("NewPerson","Movies")")
    $("#addnewproducer").show();
    // $(".addprod .form-horizontal h4").html("Producer");
    $(window).scrollTop($('#addnewproducer').offset().top)


}

var NewActor=function () {

    AddingActor = true;
    // $("#addnewactor1").load("@Url.Action("NewPerson","Movies")")
    $("#addnewactor1").show();
    $(window).scrollTop($('#addnewactor1').offset().top)
    //$(".addact .form-horizontal h4").html("Actor");
}


var ClearPreview = function () {
    $("#poster").val('');
    $("#imgpreview").hide();
};



var ReadImage = function (file) {
    var reader = new FileReader;
    image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {
        image.src = _file.target.result;
        image.onload = function () {
            var height = this.height;
            var width = this.width;
            var type = file.type;
            var size = ~~(file.size / 1024) + "KB";

            $("#targetImg").attr('src', _file.target.result);
            $("#imgpreview").show();

        }

    };
};
var addnewactor1=function(){
    name=$("#a1newname").val()
    if (!Existlist(name, "actors"))
        
        addnewactor(1);
    else
        setactor(name);
}
var addnewactor2 = function () {
    name=$("#a2newname").val()
    if(!Existlist(name,"actors"))
        addnewactor(2);
    else
        setactor(name);

                

}
var addnewactor3 = function () {
    name=$("#a3newname").val()
    if(!Existlist(name,"actors"))
        addnewactor(3);
    else
        setactor(name);

}

var addnewactor = function (id) {

    if (!validateperson("a" + id))
        return

    nameid = "#a" + id + "newname";
    sexid = "#a" + id + "newsex";
    bioid = "#a" + id + "newbio";
    dobid = "#a" + id + "newdob";
    var person = {
        name: $(nameid).val(),
        sex: $(sexid).val(),
        bio: $(bioid).val(),
        dob: $(dobid).val()
    };
    $.ajax({
        url: "/Actors/AddActor",
        method: "POST",
        data: person,
        success: function (data) {
            // alert("Actor added");
            divid = "#addnewactor" + id;
            $(divid).hide();
            console.log(data.Id);
            console.log(data.Person);
            var o = new Option(person.name, data.Id);
            $(o).html(person.name);
            $("#actors").append(o);
            //$("#actors option[value='"+data.Id+"']").prop('selected',true);
            //actorlist.concat(data.Person.Name)
            setactor(person.name); 
        },

        error: function (request, status, error) {
            alert(status + ", " + error);


        }

    });
}
var addnewproducer = function () {

    if (!validateperson("p"))
        return;
    name = $("#newname").val();
    if(!Existlist(name,"producer"))
    {
                    
        var person = {
            name: $("#newname").val(),
            sex: $("#newsex").val(),
            bio: $("#newbio").val(),
            dob: $("#newdob").val()
        };
                    
        $.ajax({
            url: "/Producers/AddProducer",
            method: "POST",
            data: person,
            success: function (data) {
                //  alert("Producer added");
                $("#addnewproducer").hide();
                console.log(data.Id);
                console.log(data.Person);
                var o = new Option(person.name, data.Id);
                $(o).html(person.name);
                $("#producer").append(o);
                $("#producer").val(data.Id).change();

            },

            error: function (request, status, error) {
                alert(status + ", " + error);


            }

        });

    }
    else{
        $("#producer option:contains(" + name + ")").attr('selected', 'selected').change();

    }


}
