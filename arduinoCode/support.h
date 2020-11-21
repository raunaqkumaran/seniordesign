#include <math.h>

//Wrapper to manage what is essentially a vector. x, y, z are the basis vectors, and magnitude is...the magnitude (except when it's blank, in which case the x,y,z are locations)
//Useful for tracking the moment correction required for a statically unbalanced system. x, y, z tell you the direction to move the weight in, magnitude tells you the amount of weight that has to be applied to attain the counter.
struct coordinates {
	double x;
	double y;
	double z;
	double magnitude;
};


//Takes an array and turns it into a coordinate. Doesn't apply a value to the magnitude of the coordinate.
coordinates coordFromArray(double arr[3])
{
    coordinates res;
    res.x = arr[0];
    res.y = arr[1];
    res.z = arr[2];

    return res;
}

//Calculates the location of the center of mass, using the forces from the load cells, and the locations of the load cells.
coordinates comLocation(double* forces, coordinates* locations, int length)
{
	coordinates com;
	double totalForce = 0;
	double xSum = 0;
	double ySum = 0;
	double zSum = 0;
	double force;
	coordinates loc;

	for (int i = 0; i < length; i++)
	{
		force = forces[i];
		loc = locations[i];

		totalForce += force;
		xSum += loc.x * force;
		ySum += loc.y * force;
		zSum += loc.z * force;
	}

  if (totalForce == 0)
  {
    totalForce = 0.001;
  }

	com.x = xSum / totalForce;
	com.y = ySum / totalForce;
	com.z = zSum / totalForce;

	return com;
}

//Calculates an additional moment that would be required to rebalance the system.
coordinates correctionMoment(double force, coordinates com)
{
	double magnitude = sqrt(pow(com.x, 2) + pow(com.y, 2) + pow(com.z, 2));
  if (magnitude == 0)
  {
    magnitude = 0.001;
  }
	coordinates correction;
	correction.x = -com.x / magnitude;
	correction.y = -com.y / magnitude;
	correction.z = -com.z / magnitude;
	correction.magnitude = force * magnitude;

	return correction;
}

//Adds up all the values in the array. (If you wanted to calculate the total sum of forces if you had an array of forces from each load cell)
double sumArr(double* arr, int length)
{
	double res = 0;
	for (int i = 0; i < length; i++)
		res += arr[i];
	return res;
}

//Calculates the radius of rotation. a = omega^2 * r. omega is known from the rotation rate of the structure. a is known from the accelerometer. r is what is returned.
double radiusOfRotation(double omega, double acceleration)
{
  if (omega == 0)
  {
    omega = 0.001;
  }
	double res = acceleration / pow(omega, 2);
	return res;
}
