#include <math.h>

struct coordinates {
	double x;
	double y;
	double z;
	double magnitude;
};


int add(int a, int b)
{
	int res = a + b;
	return res;
}

struct loadCell {
    int DOUT;
    int CLK;
    double location[3];
    double calibration;
};

coordinates coordFromArray(double arr[3])
{
    coordinates res;
    res.x = arr[0];
    res.y = arr[1];
    res.z = arr[2];

    return res;
}

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

double sumArr(double* arr, int length)
{
	double res = 0;
	for (int i = 0; i < length; i++)
		res += arr[i];
	return res;
}

double radiusOfRotation(double omega, double acceleration)
{
  if (omega == 0)
  {
    omega = 0.001;
  }
	double res = acceleration / pow(omega, 2);
	return res;
}
