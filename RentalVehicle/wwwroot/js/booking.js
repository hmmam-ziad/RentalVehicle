document.getElementById('RentalStartDate').addEventListener('change', function () {
    const inputDate = new Date(this.value);
    const currentDate = new Date();
    const input = document.getElementById("myInput");


    if (inputDate <= currentDate) {
        document.getElementById('dateError').innerText = "Rental Start Date must be in the future.";
        input.disabled = true;
    } else {
        document.getElementById('dateError').innerText = "";
        input.disabled = false;
    }
    checkVehicleAvailability()
});



document.getElementById('RentalEndDate').addEventListener('change', function () {
    const inputDate = new Date(this.value);
    const startDate = new Date(document.getElementById('RentalStartDate').value);
    const input = document.getElementById("myInput");


    if (inputDate <= startDate) {
        document.getElementById('dateError').innerText = "End Date must be greater than Start Date.";
        input.disabled = true;
    } else {
        document.getElementById('dateError').innerText = "";
        input.disabled = false;
    }
    checkVehicleAvailability()
});


function checkVehicleAvailability() {
    const vehicleId = document.getElementById("VehicleID").value;
    const rentalStartDate = document.getElementById("RentalStartDate").value;
    const rentalEndDate = document.getElementById("RentalEndDate").value;


    if (!vehicleId || !rentalStartDate || !rentalEndDate) {
        return;
    }


    const startDate = new Date(rentalStartDate).toISOString();
    const endDate = new Date(rentalEndDate).toISOString();


    $.ajax({
        url: '/RentalBookings/CheckAvailability',
        type: 'GET',
        data: {
            vehicleId: vehicleId,
            startDate: startDate,
            endDate: endDate
        },
        success: function (response) {
            const dateError = document.getElementById('dateError');
            const input = document.getElementById("myInput");

            if (response.isAvailable) {
                dateError.innerText = "";
                input.disabled = false;
            } else {

                dateError.innerText = `This vehicle is already booked from ${response.startDate} to ${response.endDate}.`;
                input.disabled = true;
            }
        },
        error: function () {
            document.getElementById('dateError').innerText = "An error occurred while checking availability.";
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    const mySelect = document.querySelector('select');
    const vehicleElement = document.getElementById('VehicleID');
    // console.log(vehicleElement.value);

    if (!mySelect || !vehicleElement) {
        console.error('Required elements (mySelect or VehicleID) not found in the DOM.');
        return;
    }

    mySelect.addEventListener('change', function () {
        const vehicleId = vehicleElement.value;
        if (!vehicleId) {
            document.getElementById('CarError').innerText = "Please select a vehicle.";
            return;
        }

        const image = document.getElementById('imageV');
        const model = document.getElementById('modelV');
        const price = document.getElementById('priceV');
        const seat = document.getElementById('seatV');
        const transmission = document.getElementById('transmissionV');
        const fule = document.getElementById('fuleV');
        const year = document.getElementById('yearV');
        const transmission1 = document.getElementById('transmissionV1');
        const mile = document.getElementById('mileV');

        $.ajax({
            url: '/RentalBookings/GetVehicleDetails',
            type: 'GET',
            data: {
                vehicleId: vehicleId
            },
            success: function (response) {
                if (response) {
                    image.src = '/VehiclesImage/' + response.image;
                    model.innerText = response.model;
                    price.innerText = response.price + '$';
                    seat.innerText = response.seat;
                    fule.innerText = response.fule;
                    transmission.innerText = response.transmission;
                    year.innerText = response.year;
                    transmission1.innerText = response.transmission;
                    mile.innerText = response.mile;
                } else {
                    document.getElementById('CarError').innerText = "Vehicle details not found.";
                }
            },
            error: function (xhr, status, error) {
                console.log('Error:', error);
                document.getElementById('CarError').innerText = "An error occurred while fetching vehicle details.";
            }
        });
    });
});