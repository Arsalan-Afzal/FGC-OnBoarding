////var template = $('#sections .section:first').clone();
////$(template).css("display", "block");
////var sectionsCount = 0;
//////add new section
////$('body').on('click', '.addsection', function () {
////    debugger
////    sectionsCount++;
////    var section = template.clone().find(':input').each(function () {
////        var newId = this.id + sectionsCount;
////        $(this).prev().attr('for', newId);
////        this.id = newId;
////        $("#addnewauth").show(300);
////    }).end()

////        .appendTo('#sections');
////    return false;
////});

//////remove section
////$('#sections').on('click', '.remove', function () {
////    $(this).parent().fadeOut(300, function () {
////        //$(this).parent().parent().empty();
////        $(this).parent().remove();
////        return false;
////    });
////    return false;
////});