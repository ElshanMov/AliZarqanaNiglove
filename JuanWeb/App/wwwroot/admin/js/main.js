$(function () {
    console.log("salam")
    $(document).on("click", ".btn-delete", function (e) {
        console.log("salam")
        e.preventDefault();
        let href = $(this).attr("href");
        console.log(href);
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(href).then(response => {
                    if (response.ok) {
                        window.location.reload(true);
                    }
                    else {
                        alert("Not Found");
                    }
                });
            }
            else {
                console.log("armud")
            }
        })
    });
});
